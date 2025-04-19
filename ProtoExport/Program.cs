using CommandLine;

namespace GameFrameX.ProtoExport
{
    internal static class Program
    {
        private static int Main(string[] args)
        {
            try
            {
                var launcherOptions = Parser.Default.ParseArguments<LauncherOptions>(args).Value;
                if (launcherOptions == null)
                {
                    Console.WriteLine("参数错误，解析失败");
                    throw new Exception("参数错误，解析失败");
                }

                if (!Enum.TryParse<ModeType>(launcherOptions.Mode, true, out var modeType))
                {
                    Console.WriteLine("不支持的运行模式");
                    throw new Exception("不支持的运行模式");
                }

                if (!Enum.TryParse<ClearOption>(launcherOptions.ClearOption, true, out var clearOption))
                {
                    Console.WriteLine("unknow clear option");
                    throw new Exception("unknow clear option");
                }

                ProtoBufMessageHandler.Start(launcherOptions, modeType, clearOption);
                Console.WriteLine("导出成功,请查看日志");
            }
            catch (Exception e)
            {
                Console.WriteLine("导出失败,请检查错误信息");
                Console.WriteLine(e);
                Console.ReadKey();
                throw;
            }

            return 0;
        }
    }
}