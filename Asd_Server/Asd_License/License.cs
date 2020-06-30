using Asd_Extensions;
using Asd_Logging;
using System;
using Asd_Crypto;
using Asd_Hardware;
using Newtonsoft.Json;

namespace Asd_License
{
    public static class License
    {
        private const string ClassName = "License";

        public static Asd_License_Model Get(Asd_Hardware_Model hardware, Asd_License_Info_Model licenseInfo, Asd_License_Person_Model licensePerson)
        {
            try
            {
                var license = new Asd_License_Model
                {
                    Hardware = new Asd_Hardware_Model
                    {
                        OperatingSystem = hardware.OperatingSystem,
                        Cpu = new Asd_Cpu_Model
                        {
                            PhysicalCores = hardware.Cpu?.PhysicalCores ?? 0,
                            LogicalCores = hardware.Cpu?.LogicalCores ?? 0,
                            Architecture = hardware.Cpu?.Architecture,
                            Name = hardware.Cpu?.Name ?? "NULL",
                            Vendor = hardware.Cpu?.Vendor ?? "NULL",
                            Model = hardware.Cpu?.Model ?? 0,
                            Family = hardware.Cpu?.Family ?? 0
                        },
                        SmBios = new Asd_SmBios_Model
                        {
                            BiosVersion = hardware.SmBios?.BiosVersion ?? "NULL",
                            BiosVendor = hardware.SmBios?.BiosVendor ?? "NULL",
                            BiosCodename = hardware.SmBios?.BiosCodename ?? "NULL",
                            BoardVendor = hardware.SmBios?.BoardVendor ?? "NULL",
                            BoardName = hardware.SmBios?.BoardName ?? "NULL",
                            BoardVersion = hardware.SmBios?.BoardVersion ?? "NULL"
                        }
                    },
                    Info = new Asd_License_Info_Model
                    {
                        Kiosks = licenseInfo.Kiosks,
                        Workplaces = licenseInfo.Workplaces,
                        Expires = licenseInfo.Expires
                    },
                    Person = new Asd_License_Person_Model
                    {
                        Name = licensePerson?.Name ?? "NULL",
                        Company = licensePerson?.Company ?? "NULL",
                        Email = licensePerson?.Email ?? "NULL",
                        Phone = licensePerson?.Phone ?? "NULL"
                    }
                };

                return !license.Correct() ? null : license;
            }
            catch
            {
                return null;
            }
        }

        public static Asd_License_Model Get(string base64)
        {
            try
            {
                var json = ExtensionProvider.Base64Decode(base64);
                var license = JsonConvert.DeserializeObject<Asd_License_Model>(json);
                return license.Correct() ? license : null;
            }
            catch
            {
                return null;
            }
        }

        private static bool Correct(this Asd_License_Model license)
        {
            try
            {
                if (license?.Info == null || license.Hardware == null) return false;
                return license.Info.Kiosks >= 1 && license.Info.Workplaces >= 1;
            }
            catch
            {
                return false;
            }
        }

        public static bool Verify(Asd_License_Model license, string correlationId)
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
                // проверяем запрос
                if (license == null)
                {
                    // логируем ответ
                    Logging.Warn(preLog + "(Результат метода: false, license == null)");
                    // возвращаем ответ
                    return false;
                }
                // получаем объект hardware текущего сервера
                var hardware = Hardware.Get(correlationId);
                if (hardware == null)
                {
                    // логируем ответ
                    Logging.Error(preLog + "(Результат метода: false, hardware == null)");
                    // возвращаем ответ
                    return false;
                }
                // создаем объект лицензии для проверки
                var lic = Get(hardware, license.Info, license.Person);
                if (lic == null)
                {
                    // логируем ответ
                    Logging.Error(preLog + "(Результат метода: false, lic == null)");
                    // возвращаем ответ
                    return false;
                }
                // преобразуем объект lic в строку json
                var json = JsonConvert.SerializeObject(lic, Formatting.Indented).ToTrimString();
                // преобразуем строку json в base64
                var base64 = ExtensionProvider.Base64Encode(json);
                // проверяем совпадение хеш-сумм
                if (!CryptoProvider.VerifyHash(license.Hash, base64))
                {
                    // логируем ответ
                    Logging.Warn(preLog + "(Результат метода: false, !CryptoProvider.VerifyHash)");
                    // возвращаем ответ
                    return false;
                }
                // проверяем срок действия лицензии
                if (lic.Info.Expires != "безсрочная")
                {
                    if (!DateTime.TryParse(lic.Info.Expires, out var licExpires))
                    {
                        // логируем ответ
                        Logging.Error(preLog + "(Результат метода: false, !DateTime.TryParse)");
                        // возвращаем ответ
                        return false;
                    }
                    if (licExpires < DateTime.Now)
                    {
                        // логируем ответ
                        Logging.Warn(preLog + "(Результат метода: false, licExpires < DateTime.Now)");
                        // возвращаем ответ
                        return false;
                    }
                }
                // логируем ответ
                Logging.Response(preLog + "(Ответ метода: true)" + JsonConvert.SerializeObject(hardware, Formatting.Indented).ToTabString() + JsonConvert.SerializeObject(lic, Formatting.Indented).ToTabString());
                // возвращаем ответ
                return true;
            }
            catch (Exception ex)
            {
                // логируем исключение
                Logging.Exception(preLog + "(" + ex.Message + ")" + ex.StackTrace.ToTabString());
                // возвращаем ответ
                return false;
            }
            finally
            {
                // логируем окончание выполнения метода
                Logging.Trace(preLog + " END");
            }
        }
    }
}
