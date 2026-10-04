using System.Globalization;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Nkklession14layout.Data;
using Nkklession14layout.Models;

namespace Nkklession14layout.Controllers;

[Authorize]
public class NkkShopController(NkkApplicationDbContext db, ILogger<NkkShopController> logger) : Controller
{
    private static readonly IReadOnlyDictionary<string, (Type Type, string Title)> Entities =
        new Dictionary<string, (Type, string)>(StringComparer.OrdinalIgnoreCase)
        {
            ["dien-thoai"] = (typeof(NkkDienThoaiModel), "Điện thoại"),
            ["nha-cung-cap"] = (typeof(NkkNhaCungCapModel), "Nhà cung cấp"),
            ["khach-hang"] = (typeof(NkkKhachHangModel), "Khách hàng"),
            ["nhan-vien"] = (typeof(NkkNhanVienModel), "Nhân viên"),
            ["hoa-don"] = (typeof(NkkHoaDonModel), "Hóa đơn"),
            ["chi-tiet-hoa-don"] = (typeof(NkkChiTietHoaDonModel), "Chi tiết hóa đơn"),
            ["phieu-nhap"] = (typeof(NkkPhieuNhapModel), "Phiếu nhập"),
            ["chi-tiet-phieu-nhap"] = (typeof(NkkChiTietPhieuNhapModel), "Chi tiết phiếu nhập")
        };

    [HttpGet]
    public IActionResult NkkIndex() => RedirectToAction(nameof(NkkList), new { entity = "dien-thoai" });

    [HttpGet]
    public async Task<IActionResult> NkkList(string entity)
    {
        if (!NkkTryGetEntity(entity, out var canonicalEntity, out var type, out var title))
        {
            return NotFound();
        }

        entity = canonicalEntity;
        var entityType = db.Model.FindEntityType(type)!;
        var properties = entityType.GetProperties().ToArray();
        var records = await NkkLoadEntitiesAsync(entity);
        var rows = records.Select(record => new NkkShopListRow
        {
            Key = NkkEncodeKey(entityType.FindPrimaryKey()!.Properties
                .Select(property => property.PropertyInfo!.GetValue(record)?.ToString() ?? "")
                .ToArray()),
            Values = properties.Select(property => NkkFormatValue(property.PropertyInfo!.GetValue(record))).ToArray()
        }).ToArray();

        return View("NkkList", new NkkShopListViewModel
        {
            Entity = entity,
            Title = title,
            Columns = properties.Select(property => NkkGetColumnName(property, entityType)).ToArray(),
            Rows = rows
        });
    }

    [HttpGet]
    public IActionResult NkkCreate(string entity)
    {
        if (!NkkTryGetEntity(entity, out var canonicalEntity, out var type, out var title))
        {
            return NotFound();
        }

        entity = canonicalEntity;
        var model = NkkBuildEditor(entity, type, title, Activator.CreateInstance(type)!, null);
        return View("NkkEdit", model);
    }

    [HttpGet]
    public async Task<IActionResult> NkkEdit(string entity, string key)
    {
        if (!NkkTryGetEntity(entity, out var canonicalEntity, out var type, out var title) ||
            !NkkTryDecodeKey(key, db.Model.FindEntityType(type)!.FindPrimaryKey()!.Properties.Count, out var keyValues))
        {
            return NotFound();
        }

        entity = canonicalEntity;
        var record = await NkkFindEntityAsync(entity, keyValues);
        return record is null
            ? NotFound()
            : View("NkkEdit", NkkBuildEditor(entity, type, title, record, key));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> NkkSave(string entity, string? key)
    {
        if (!NkkTryGetEntity(entity, out var canonicalEntity, out var type, out var title))
        {
            return NotFound();
        }

        entity = canonicalEntity;
        var entityType = db.Model.FindEntityType(type)!;
        var keyProperties = entityType.FindPrimaryKey()!.Properties;
        object record;
        if (string.IsNullOrWhiteSpace(key))
        {
            record = Activator.CreateInstance(type)!;
        }
        else
        {
            if (!NkkTryDecodeKey(key, keyProperties.Count, out var keyValues))
            {
                return BadRequest();
            }

            record = await NkkFindEntityAsync(entity, keyValues) ?? throw new InvalidOperationException("The requested record no longer exists.");
        }

        var form = await Request.ReadFormAsync(HttpContext.RequestAborted);
        foreach (var property in entityType.GetProperties())
        {
            if (!string.IsNullOrWhiteSpace(key) && keyProperties.Contains(property))
            {
                continue;
            }

            var propertyInfo = property.PropertyInfo!;
            if (!form.TryGetValue(property.Name, out var input))
            {
                continue;
            }

            if (!NkkTryConvert(input.ToString(), propertyInfo.PropertyType, out var converted))
            {
                ModelState.AddModelError(property.Name, $"Giá trị của {NkkGetColumnName(property, entityType)} không hợp lệ.");
                continue;
            }

            propertyInfo.SetValue(record, converted);
        }

        TryValidateModel(record);
        if (!ModelState.IsValid)
        {
            return View("NkkEdit", NkkBuildEditor(entity, type, title, record, key));
        }

        if (string.IsNullOrWhiteSpace(key))
        {
            NkkAddEntity(entity, record);
        }

        try
        {
            await db.SaveChangesAsync();
            TempData["Success"] = $"{title} đã được lưu.";
            return RedirectToAction(nameof(NkkList), new { entity });
        }
        catch (DbUpdateException exception)
        {
            logger.LogWarning(exception, "Saving a {Entity} record failed due to a database constraint.", entity);
            ModelState.AddModelError(string.Empty, "Không thể lưu dữ liệu. Kiểm tra mã khóa chính, các trường bắt buộc và mã tham chiếu có tồn tại.");
            return View("NkkEdit", NkkBuildEditor(entity, type, title, record, key));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> NkkDelete(string entity, string key)
    {
        if (!NkkTryGetEntity(entity, out var canonicalEntity, out _, out var title))
        {
            return NotFound();
        }

        entity = canonicalEntity;
        var primaryKey = db.Model.FindEntityType(Entities[entity].Type)!.FindPrimaryKey()!;
        if (!NkkTryDecodeKey(key, primaryKey.Properties.Count, out var keyValues))
        {
            return BadRequest();
        }

        var record = await NkkFindEntityAsync(entity, keyValues);
        if (record is null)
        {
            TempData["Error"] = "Bản ghi không còn tồn tại.";
            return RedirectToAction(nameof(NkkList), new { entity });
        }

        NkkRemoveEntity(entity, record);
        try
        {
            await db.SaveChangesAsync();
            TempData["Success"] = $"{title} đã được xóa.";
        }
        catch (DbUpdateException exception)
        {
            logger.LogWarning(exception, "Deleting a {Entity} record failed due to related records.", entity);
            TempData["Error"] = "Không thể xóa bản ghi vì đang được dữ liệu khác tham chiếu.";
        }

        return RedirectToAction(nameof(NkkList), new { entity });
    }

    private NkkShopEditViewModel NkkBuildEditor(string entity, Type type, string title, object record, string? key)
    {
        var entityType = db.Model.FindEntityType(type)!;
        var primaryKeys = entityType.FindPrimaryKey()!.Properties;
        return new NkkShopEditViewModel
        {
            Entity = entity,
            Title = title,
            Key = key,
            Fields = entityType.GetProperties().Select(property =>
            {
                var propertyInfo = property.PropertyInfo!;
                var typeName = Nullable.GetUnderlyingType(propertyInfo.PropertyType)?.Name ?? propertyInfo.PropertyType.Name;
                var inputType = typeName switch
                {
                    "DateTime" => property.GetColumnType() == "date" ? "date" : "datetime-local",
                    "Decimal" => "number",
                    "Int32" => "number",
                    _ => "text"
                };
                var required = !property.IsNullable && propertyInfo.PropertyType != typeof(string) ||
                    propertyInfo.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.RequiredAttribute), true).Length != 0;
                return new NkkShopEditField
                {
                    Name = property.Name,
                    Label = NkkGetColumnName(property, entityType),
                    Value = NkkFormatInputValue(propertyInfo.GetValue(record), property.GetColumnType()),
                    InputType = inputType,
                    Step = typeName == "Decimal" ? "0.01" : typeName == "Int32" ? "1" : null,
                    MaxLength = property.GetMaxLength(),
                    IsKey = primaryKeys.Contains(property),
                    IsRequired = required
                };
            }).ToArray()
        };
    }

    private async Task<List<object>> NkkLoadEntitiesAsync(string entity) => entity switch
    {
        "dien-thoai" => (await db.DienThoais.AsNoTracking().ToListAsync()).Cast<object>().ToList(),
        "nha-cung-cap" => (await db.NhaCungCaps.AsNoTracking().ToListAsync()).Cast<object>().ToList(),
        "khach-hang" => (await db.KhachHangs.AsNoTracking().ToListAsync()).Cast<object>().ToList(),
        "nhan-vien" => (await db.NhanViens.AsNoTracking().ToListAsync()).Cast<object>().ToList(),
        "hoa-don" => (await db.HoaDons.AsNoTracking().ToListAsync()).Cast<object>().ToList(),
        "chi-tiet-hoa-don" => (await db.ChiTietHoaDons.AsNoTracking().ToListAsync()).Cast<object>().ToList(),
        "phieu-nhap" => (await db.PhieuNhaps.AsNoTracking().ToListAsync()).Cast<object>().ToList(),
        "chi-tiet-phieu-nhap" => (await db.ChiTietPhieuNhaps.AsNoTracking().ToListAsync()).Cast<object>().ToList(),
        _ => throw new InvalidOperationException("Unknown shop entity.")
    };

    private async Task<object?> NkkFindEntityAsync(string entity, object?[] keys) => entity switch
    {
        "dien-thoai" => await db.DienThoais.FindAsync(keys).AsTask(),
        "nha-cung-cap" => await db.NhaCungCaps.FindAsync(keys).AsTask(),
        "khach-hang" => await db.KhachHangs.FindAsync(keys).AsTask(),
        "nhan-vien" => await db.NhanViens.FindAsync(keys).AsTask(),
        "hoa-don" => await db.HoaDons.FindAsync(keys).AsTask(),
        "chi-tiet-hoa-don" => await db.ChiTietHoaDons.FindAsync(keys).AsTask(),
        "phieu-nhap" => await db.PhieuNhaps.FindAsync(keys).AsTask(),
        "chi-tiet-phieu-nhap" => await db.ChiTietPhieuNhaps.FindAsync(keys).AsTask(),
        _ => throw new InvalidOperationException("Unknown shop entity.")
    };

    private void NkkAddEntity(string entity, object record)
    {
        switch (entity)
        {
            case "dien-thoai": db.DienThoais.Add((NkkDienThoaiModel)record); break;
            case "nha-cung-cap": db.NhaCungCaps.Add((NkkNhaCungCapModel)record); break;
            case "khach-hang": db.KhachHangs.Add((NkkKhachHangModel)record); break;
            case "nhan-vien": db.NhanViens.Add((NkkNhanVienModel)record); break;
            case "hoa-don": db.HoaDons.Add((NkkHoaDonModel)record); break;
            case "chi-tiet-hoa-don": db.ChiTietHoaDons.Add((NkkChiTietHoaDonModel)record); break;
            case "phieu-nhap": db.PhieuNhaps.Add((NkkPhieuNhapModel)record); break;
            case "chi-tiet-phieu-nhap": db.ChiTietPhieuNhaps.Add((NkkChiTietPhieuNhapModel)record); break;
            default: throw new InvalidOperationException("Unknown shop entity.");
        }
    }

    private void NkkRemoveEntity(string entity, object record)
    {
        switch (entity)
        {
            case "dien-thoai": db.DienThoais.Remove((NkkDienThoaiModel)record); break;
            case "nha-cung-cap": db.NhaCungCaps.Remove((NkkNhaCungCapModel)record); break;
            case "khach-hang": db.KhachHangs.Remove((NkkKhachHangModel)record); break;
            case "nhan-vien": db.NhanViens.Remove((NkkNhanVienModel)record); break;
            case "hoa-don": db.HoaDons.Remove((NkkHoaDonModel)record); break;
            case "chi-tiet-hoa-don": db.ChiTietHoaDons.Remove((NkkChiTietHoaDonModel)record); break;
            case "phieu-nhap": db.PhieuNhaps.Remove((NkkPhieuNhapModel)record); break;
            case "chi-tiet-phieu-nhap": db.ChiTietPhieuNhaps.Remove((NkkChiTietPhieuNhapModel)record); break;
            default: throw new InvalidOperationException("Unknown shop entity.");
        }
    }

    private static bool NkkTryGetEntity(string? entity, out string canonicalEntity, out Type type, out string title)
    {
        if (entity is not null && Entities.TryGetValue(entity, out var metadata))
        {
            canonicalEntity = Entities.First(pair => pair.Value.Type == metadata.Type).Key;
            type = metadata.Type;
            title = metadata.Title;
            return true;
        }

        canonicalEntity = "";
        type = typeof(void);
        title = "";
        return false;
    }

    private static bool NkkTryConvert(string value, Type propertyType, out object? result)
    {
        var targetType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;
        if (string.IsNullOrWhiteSpace(value) && Nullable.GetUnderlyingType(propertyType) is not null)
        {
            result = null;
            return true;
        }

        try
        {
            result = targetType == typeof(string) ? value :
                targetType == typeof(DateTime)
                    ? DateTime.Parse(value, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind)
                    : Convert.ChangeType(value, targetType, CultureInfo.InvariantCulture);
            return true;
        }
        catch (FormatException)
        {
            result = null;
            return false;
        }
        catch (OverflowException)
        {
            result = null;
            return false;
        }
    }

    private static string NkkGetColumnName(IProperty property, IEntityType entityType) =>
        property.GetColumnName(StoreObjectIdentifier.Table(entityType.GetTableName()!, entityType.GetSchema()))
        ?? property.Name;

    private static string? NkkFormatValue(object? value) => value switch
    {
        null => "",
        DateTime date => date.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
        decimal amount => amount.ToString("0.00", CultureInfo.InvariantCulture),
        _ => Convert.ToString(value, CultureInfo.InvariantCulture)
    };

    private static string NkkFormatInputValue(object? value, string? columnType) => value switch
    {
        null => "",
        DateTime date when columnType == "date" => date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
        DateTime date when columnType == "datetime" => date.ToString("yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture),
        decimal amount => amount.ToString("0.00", CultureInfo.InvariantCulture),
        _ => Convert.ToString(value, CultureInfo.InvariantCulture) ?? ""
    };

    private static string NkkEncodeKey(string[] keys) =>
        WebEncoders.Base64UrlEncode(JsonSerializer.SerializeToUtf8Bytes(keys));

    private static bool NkkTryDecodeKey(string encoded, int expectedKeyCount, out object?[] keys)
    {
        try
        {
            var decoded = JsonSerializer.Deserialize<string[]>(WebEncoders.Base64UrlDecode(encoded));
            if (decoded is null || decoded.Length != expectedKeyCount || decoded.Any(string.IsNullOrWhiteSpace))
            {
                keys = [];
                return false;
            }

            keys = decoded.Cast<object?>().ToArray();
            return true;
        }
        catch (FormatException)
        {
            keys = [];
            return false;
        }
        catch (JsonException)
        {
            keys = [];
            return false;
        }
    }
}
