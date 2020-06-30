using System;
using Asd_Extensions;
using Asd_Logging;
using HardwareInformation;
using Newtonsoft.Json;

namespace Asd_Hardware
{
    public static class Hardware
    {
        private const string ClassName = "Hardware";

        public static Asd_Hardware_Model Get(string correlationId)
        {
            // информация для логирования
            var preLog = correlationId + " | " + ClassName + "." + ExtensionProvider.GetCallerMethodName();
            // логируем начало выполнения метода
            Logging.Trace(preLog + " START");
            // выполнение метода
            try
            {
                // логируем запрос
                Logging.Request(preLog + "(Запрос метода.)");
                // получаем информацию о сервере
                var info = MachineInformationGatherer.GatherInformation();
                if (info == null)
                {
                    // логируем ответ
                    Logging.Error(preLog + "(Результат метода: null, info == null)");
                    // возвращаем ответ
                    return null;
                }
                // формируем ответ
                var hardware = new Asd_Hardware_Model
                {
                    OperatingSystem = info.OperatingSystem?.VersionString ?? "NULL",
                    Cpu = new Asd_Cpu_Model
                    {
                        PhysicalCores = info.Cpu?.PhysicalCores ?? 0,
                        LogicalCores = info.Cpu?.LogicalCores ?? 0,
                        Architecture = info.Cpu?.Architecture ?? "NULL",
                        Name = info.Cpu?.Name ?? "NULL",
                        Vendor = info.Cpu?.Vendor ?? "NULL",
                        Model = info.Cpu?.Model ?? 0,
                        Family = info.Cpu?.Family ?? 0
                    },
                    SmBios = new Asd_SmBios_Model
                    {
                        BiosVersion = info.SmBios?.BIOSVersion ?? "NULL",
                        BiosVendor = info.SmBios?.BIOSVendor ?? "NULL",
                        BiosCodename = info.SmBios?.BIOSCodename ?? "NULL",
                        BoardVendor = info.SmBios?.BoardVendor ?? "NULL",
                        BoardName = info.SmBios?.BoardName ?? "NULL",
                        BoardVersion = info.SmBios?.BoardVersion ?? "NULL"
                    }
                };
                // проверяем ответ
                if (hardware.OperatingSystem == "NULL" &&
                    hardware.Cpu.PhysicalCores == 0 &&
                    hardware.Cpu.LogicalCores == 0 &&
                    hardware.Cpu.Architecture == "NULL" &&
                    hardware.Cpu.Name == "NULL" &&
                    hardware.Cpu.Vendor == "NULL" &&
                    hardware.Cpu.Model == 0 &&
                    hardware.Cpu.Family == 0 &&
                    hardware.SmBios.BiosVersion == "NULL" &&
                    hardware.SmBios.BiosVendor == "NULL" &&
                    hardware.SmBios.BiosCodename == "NULL" &&
                    hardware.SmBios.BoardVendor == "NULL" &&
                    hardware.SmBios.BoardName == "NULL" &&
                    hardware.SmBios.BoardVersion == "NULL")
                {
                    // логируем ответ
                    Logging.Error(preLog + "(результат метода: null, hardware == null)");
                    // возвращаем ответ
                    return null;
                }
                // логируем ответ
                Logging.Response(preLog + "(Ответ метода.)" + JsonConvert.SerializeObject(hardware, Formatting.Indented).ToTabString());
                // возвращаем ответ
                return hardware;
            }
            catch (Exception ex)
            {
                // логируем исключение
                Logging.Exception(preLog + "(" + ex.Message + ")" + ex.StackTrace.ToTabString());
                // возвращаем ответ
                return null;
            }
            finally
            {
                // логируем окончание выполнения метода
                Logging.Trace(preLog + " END");
            }
        }
    }
}
