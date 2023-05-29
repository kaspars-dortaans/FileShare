namespace FileShare.Enums;

public enum SettingType
{
    FileExtensions,
    MaxFileSize,
    MinImageSize,
    MaxImageSize
}

public enum SettingDataType
{
    String,
    Size,
    StringList,
    PositiveInteger
}

public static class AppSettings
{
    public static Dictionary<SettingType, (string name, SettingDataType dataType)> Settings = new(){
        { SettingType.FileExtensions, ("FileExtensions", SettingDataType.StringList)},
        { SettingType.MaxFileSize, ("MaxFileSize", SettingDataType.PositiveInteger)},
        { SettingType.MinImageSize, ("MinImageSize", SettingDataType.Size)},
        {SettingType.MaxImageSize, ("MaxImageSize", SettingDataType.Size) }
    };
}