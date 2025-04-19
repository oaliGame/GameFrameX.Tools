﻿namespace GameFrameX.ProtoExport;

public static class ProtoBufMessageHandler
{
    private static IProtoGenerateHelper ProtoGenerateHelper;

    public static void Start(LauncherOptions launcherOptions, ModeType modeType, ClearOption clearOption)
    {
        var outputDirectoryInfo = new DirectoryInfo(launcherOptions.OutputPath);
        if (!outputDirectoryInfo.Exists)
        {
            outputDirectoryInfo.Create();
        }
        if (clearOption == ClearOption.CSFile)
        {
            foreach (var fileInfo in outputDirectoryInfo.EnumerateFiles("*.cs", SearchOption.TopDirectoryOnly))
            {
                try
                {
                    File.Delete(fileInfo.FullName);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
        else if (clearOption == ClearOption.All)
        {
            foreach (var fileInfo in outputDirectoryInfo.EnumerateFiles("*", SearchOption.TopDirectoryOnly))
            {
                try
                {
                    File.Delete(fileInfo.FullName);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        launcherOptions.OutputPath = outputDirectoryInfo.FullName;

        var types = typeof(IProtoGenerateHelper).Assembly.GetTypes();
        foreach (var type in types)
        {
            var attrs = type.GetCustomAttributes(typeof(ModeAttribute), true);
            if (attrs?.Length > 0 && (attrs[0] is ModeAttribute modeAttribute) && modeAttribute.Mode == modeType)
            {
                ProtoGenerateHelper = (IProtoGenerateHelper)Activator.CreateInstance(type);
                break;
            }
        }

        var files = Directory.GetFiles(launcherOptions.InputPath, "*.proto", SearchOption.AllDirectories);

        var messageInfoLists = new List<MessageInfoList>(files.Length);
        foreach (var file in files)
        {
            var operationCodeInfo = MessageHelper.Parse(File.ReadAllText(file), Path.GetFileNameWithoutExtension(file), launcherOptions.OutputPath, launcherOptions.IsGenerateErrorCode);
            messageInfoLists.Add(operationCodeInfo);
            switch (modeType)
            {
                case ModeType.Server:
                case ModeType.Unity:
                    ProtoGenerateHelper?.Run(operationCodeInfo, launcherOptions.OutputPath, launcherOptions.NamespaceName);
                    break;
                case ModeType.TypeScript:
                    ProtoGenerateHelper?.Run(operationCodeInfo, launcherOptions.OutputPath, Path.GetFileNameWithoutExtension(file));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        ProtoGenerateHelper?.Post(messageInfoLists, launcherOptions.OutputPath);
    }
}