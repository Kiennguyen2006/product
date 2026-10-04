namespace Nkklession14layout.Models;

public class NkkShopListViewModel
{
    public string Entity { get; set; } = "";
    public string Title { get; set; } = "";
    public IReadOnlyList<string> Columns { get; set; } = [];
    public IReadOnlyList<NkkShopListRow> Rows { get; set; } = [];
}

public class NkkShopListRow
{
    public string Key { get; set; } = "";
    public IReadOnlyList<string?> Values { get; set; } = [];
}

public class NkkShopEditViewModel
{
    public string Entity { get; set; } = "";
    public string Title { get; set; } = "";
    public string? Key { get; set; }
    public IReadOnlyList<NkkShopEditField> Fields { get; set; } = [];
}

public class NkkShopEditField
{
    public string Name { get; set; } = "";
    public string Label { get; set; } = "";
    public string Value { get; set; } = "";
    public string InputType { get; set; } = "text";
    public string? Step { get; set; }
    public int? MaxLength { get; set; }
    public bool IsKey { get; set; }
    public bool IsRequired { get; set; }
}

