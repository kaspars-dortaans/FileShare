namespace FileShare.Enums;

public enum SettingType
{
    FileExtensions,
    MaxFileSize,
    MinImmageSize
}

public enum SettingDataType
{
    String,
    Size,
    StringList
}

public static class AppSettings
{
    public static Dictionary<SettingType, (string name, SettingDataType dataType)> Settings = new(){
        { SettingType.FileExtensions, ("FileExtensions", SettingDataType.StringList)},
        { SettingType.MaxFileSize, ("MaxFileSize", SettingDataType.Size)},
        { SettingType.MinImmageSize, ("MinImmageSize", SettingDataType.Size)}
    };
}