﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise
{
    public class SentinelData
    {
        public static Dictionary<string, Dictionary<string, string>> errors = new Dictionary<string, Dictionary<string, string>>();
        public static Dictionary<string, string> vendorCode = new Dictionary<string, string> { { "DEMOMA", "AzIceaqfA1hX5wS+M8cGnYh5ceevUnOZIzJBbXFD6dgf3tBkb9cvUF/Tkd/iKu2fsg9wAysYKw7RMAsVvIp4KcXle/v1RaXrLVnNBJ2H2DmrbUMOZbQUFXe698qmJsqNpLXRA367xpZ54i8kC5DTXwDhfxWTOZrBrh5sRKHcoVLumztIQjgWh37AzmSd1bLOfUGI0xjAL9zJWO3fRaeB0NS2KlmoKaVT5Y04zZEc06waU2r6AU2Dc4uipJqJmObqKM+tfNKAS0rZr5IudRiC7pUwnmtaHRe5fgSI8M7yvypvm+13Wm4Gwd4VnYiZvSxf8ImN3ZOG9wEzfyMIlH2+rKPUVHI+igsqla0Wd9m7ZUR9vFotj1uYV0OzG7hX0+huN2E/IdgLDjbiapj1e2fKHrMmGFaIvI6xzzJIQJF9GiRZ7+0jNFLKSyzX/K3JAyFrIPObfwM+y+zAgE1sWcZ1YnuBhICyRHBhaJDKIZL8MywrEfB2yF+R3k9wFG1oN48gSLyfrfEKuB/qgNp+BeTruWUk0AwRE9XVMUuRbjpxa4YA67SKunFEgFGgUfHBeHJTivvUl0u4Dki1UKAT973P+nXy2O0u239If/kRpNUVhMg8kpk7s8i6Arp7l/705/bLCx4kN5hHHSXIqkiG9tHdeNV8VYo5+72hgaCx3/uVoVLmtvxbOIvo120uTJbuLVTvT8KtsOlb3DxwUrwLzaEMoAQAFk6Q9bNipHxfkRQER4kR7IYTMzSoW5mxh3H9O8Ge5BqVeYMEW36q9wnOYfxOLNw6yQMf8f9sJN4KhZty02xm707S7VEfJJ1KNq7b5pP/3RjE0IKtB2gE6vAPRvRLzEohu0m7q1aUp8wAvSiqjZy7FLaTtLEApXYvLvz6PEJdj4TegCZugj7c8bIOEqLXmloZ6EgVnjQ7/ttys7VFITB3mazzFiyQuKf4J6+b/a/Y" } };
        public static string appSentinelUpCallData = "<upclient>" +
                                                         "<param>" +
                                                             "<key>-url</key>" +
                                                             "<value>up.sentinelcloud.com</value>" +
                                                         "</param>" +
                                                         "<param>" +
                                                             "<key>-k</key>" +
                                                             "<value>eafe87d22cc2a49793276f4141c0ebb0</value>" +
                                                         "</param>" +
                                                         "<param>" +
                                                             "<key>-pc</key>" +
                                                             "<value>1337e5b0-fd2d-4018-8a13-a921bcbfb5d2</value>" +
                                                         "</param>" +
                                                         "<param>" +
                                                             "<key>-v</key>" +
                                                             "<value>v1</value>" +
                                                         "</param>" +
                                                         "<param>" +
                                                             "<key>-update</key>" +
                                                             "<value>-update</value>" +
                                                         "</param>" +
                                                         "<param>" +
                                                             "<key>-log</key>" +
                                                             "<value>update.log</value>" +
                                                         "</param>" +
                                                     "</upclient>";
        public static string keyScope = "<haspscope>" +
                                            "<feature>" +
                                                "<name>Accounting</name>" +
                                                "<id>1</id>" +
                                            "</feature>" +
                                            "<feature>" +
                                                "<name>Stock</name>" +
                                                "<id>2</id>" +
                                            "</feature>" +
                                            "<feature>" +
                                                "<name>Staff</name>" +
                                                "<id>3</id>" +
                                            "</feature>" +
                                        "</haspscope>";

        public static string keyScopeXsd = "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
                                            "<xs:schema attributeFormDefault=\"unqualified\" elementFormDefault=\"qualified\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\">" +
                                              "<xs:element name=\"haspscope\">" +
                                                "<xs:complexType>" +
                                                  "<xs:sequence>" +
                                                    "<xs:element maxOccurs=\"unbounded\" name=\"feature\">" +
                                                      "<xs:complexType>" +
                                                        "<xs:sequence>" +
                                                          "<xs:element name=\"name\" type=\"xs:string\"/>" +
                                                          "<xs:element name=\"id\" type=\"xs:string\"/>" +
                                                        "</xs:sequence>" +
                                                      "</xs:complexType>" +
                                                    "</xs:element>" +
                                                  "</xs:sequence>" +
                                                "</xs:complexType>" +
                                              "</xs:element>" +
                                            "</xs:schema>";

        public static string keyFormat = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                                            "<haspformat root=\"hasp_info\">" +
                                                "<hasp>" +
                                                    "<element name=\"id\"/>" +
                                                    "<element name=\"type\"/>" +
                                                    "<element name=\"version\"/>" + 
                                                    "<element name=\"hw_version\"/>" +
                                                    "<element name=\"key_model\"/>" +
                                                    "<element name=\"key_type\"/>" +
                                                    "<element name=\"form_factor\"/>" +
                                                    "<element name=\"hw_platform\"/>" +
                                                    "<element name=\"driverless\"/>" +
                                                    "<element name=\"fingerprint_change\"/>" +
                                                    "<element name=\"vclock_enabled\"/>" +
		                                            "<product>" +
			                                            "<element name=\"id\"/>" +
                                                        "<element name=\"name\"/>" +
			                                            "<feature>" +
				                                            "<element name=\"id\"/>" +
                                                            "<element name=\"name\"/>" +
				                                            "<element name=\"license\"/>" +
                                                            "<element name=\"locked\"/>" +
			                                            "</feature>" +
		                                            "</product>" +
                                                "</hasp>" +
                                            "</haspformat>";
        public static string emsUrl = "http://localhost:8080/ems/v78/ws";

        public static bool logIsEnabled = true;

        public static bool apiIsEnabled = true;

        public static string portForTestConnection = "8080";

        public SentinelData() {
            // ошибки от Sentinel EMS
            errors.Add("Some unexpected error occurred in getting pending updates.Unable to retrieve Protection Key for Key ID - ", new Dictionary<string, string> { { "Ru", "Произошла непредвиденная ошибка при запросе обновлений. Невозможно получить PK для используемого ключа защиты. Вероятно ключа с таким Key ID нет в базе Sentinel EMS." }, { "En", "Some unexpected error occurred in getting pending updates.Unable to retrieve Protection Key for current Key ID. Probably there is no key with such Key ID in the Sentinel EMS database." } });
            errors.Add("No pending update.", new Dictionary<string, string> { { "Ru", "Нет доступных обновлений." }, { "En", "Have no pending update for download." } });
            //errors.Add("", new Dictionary<string, string> { { "Ru", "" }, { "En", "" } });

            // ошибки от Licensing API
            errors.Add("StatusOk", new Dictionary<string, string> { { "Ru", "Запрос был успешно выполнен!" }, { "En", "Request was successfully completed!" } });
            errors.Add("InvalidAddress", new Dictionary<string, string> { { "Ru", "Запрос превышает диапазон достпной памяти ключа защиты Sentinel." }, { "En", "Request exceeds the Sentinel protection key memory range." } });
            errors.Add("InvalidFeature", new Dictionary<string, string> { { "Ru", "Legacy HASP HL ​​API времени выполнения: неизвестный / недопустимый параметр идентификатора функции." }, { "En", "Legacy HASP HL Run-time API: Unknown/Invalid Feature ID option." } });
            errors.Add("NotEnoughMemory", new Dictionary<string, string> { { "Ru", "Недостаточно памяти." }, { "En", "System is out of memory." } });
            errors.Add("TooManyOpenFeatures", new Dictionary<string, string> { { "Ru", "Слишком много открытых сеансов." }, { "En", "Too many open sessions exist." } });
            errors.Add("AccessDenied", new Dictionary<string, string> { { "Ru", "Доступ к функции запрещен." }, { "En", "Access to Feature was denied." } });
            errors.Add("IncompatibleFeature", new Dictionary<string, string> { { "Ru", "Метод устаревшего дешифрования не может работать в функции." }, { "En", "Legacy decryption method cannot work on Feature." } });
            errors.Add("ContainerNotFound", new Dictionary<string, string> { { "Ru", "Защитный ключ Sentinel больше не доступен." }, { "En", "Sentinel protection key is no longer available." } });
            errors.Add("BufferTooShort", new Dictionary<string, string> { { "Ru", "Зашифрованная / дешифрованная длина данных слишком короткая для выполнения вызова метода." }, { "En", "Encrypted/decrypted data length too short to execute method call." } });
            errors.Add("InvalidHandle", new Dictionary<string, string> { { "Ru", "Недопустимый дескриптор был передан методу." }, { "En", "Invalid handle was passed to method." } });
            errors.Add("InvalidFile", new Dictionary<string, string> { { "Ru", "Указанный идентификатор файла не распознается API." }, { "En", "Specified File ID is not recognized by API." } });
            errors.Add("DriverTooOld", new Dictionary<string, string> { { "Ru", "Установленный драйвер слишком стар, чтобы выполнить метод." }, { "En", "Installed driver is too old to execute method." } });
            errors.Add("NoTime", new Dictionary<string, string> { { "Ru", "Часы реального времени (RTC) недоступны." }, { "En", "Real-time clock (RTC) not available." } });
            errors.Add("SystemError", new Dictionary<string, string> { { "Ru", "Общая ошибка при вызове хост-системы." }, { "En", "Generic error from host system call." } });
            errors.Add("DriverNotFound", new Dictionary<string, string> { { "Ru", "Требуемый драйвер не установлен." }, { "En", "Required driver is not installed." } });
            errors.Add("InvalidFormat", new Dictionary<string, string> { { "Ru", "Формат файла для обновления не распознается." }, { "En", "File format for update is not recognized." } });
            errors.Add("RequestNotSupported", new Dictionary<string, string> { { "Ru", "Невозможно выполнить метод в этом контексте." }, { "En", "Unable to execute method in this context." } });
            errors.Add("InvalidUpdateObject", new Dictionary<string, string> { { "Ru", "Двоичные данные, которые были переданы методу, не содержат обновления." }, { "En", "Binary data that was passed to method does not contain an update." } });
            errors.Add("KeyIdNotFound", new Dictionary<string, string> { { "Ru", "Защитный ключ Sentinel не найден." }, { "En", "Sentinel protection key was not found." } });
            errors.Add("InvalidUpdateData", new Dictionary<string, string> { { "Ru", "Необходимые теги XML не найдены." + Environment.NewLine + "ИЛИ" + Environment.NewLine + "Содержимое двоичных данных отсутствует или недействительно." }, { "En", "Required XML tags were not found." + Environment.NewLine + "OR" + Environment.NewLine + "Contents in binary data are missing or invalid." } });
            errors.Add("UpdateNotSupported", new Dictionary<string, string> { { "Ru", "Запрос обновления не поддерживается ключом защиты Sentinel." }, { "En", "Update request is not supported by Sentinel protection key." } });
            errors.Add("InvalidUpdateCounter", new Dictionary<string, string> { { "Ru", "Счетчик обновлений установлен неправильно." }, { "En", "Update counter is not set correctly." } });
            errors.Add("InvalidVendorCode", new Dictionary<string, string> { { "Ru", "Недействительный код поставщика." }, { "En", "Invalid Vendor Code was passed." } });
            errors.Add("EncryptionNotSupported", new Dictionary<string, string> { { "Ru", "Защитный ключ Sentinel не поддерживает тип шифрования." }, { "En", "Sentinel protection key does not support encryption type." } });
            errors.Add("InvalidTime", new Dictionary<string, string> { { "Ru", "Полученное значение времени находится за пределами поддерживаемого диапазона значений." }, { "En", "The time value that was passed is outside the supported value range." } });
            errors.Add("NoBatteryPower", new Dictionary<string, string> { { "Ru", "Батарея часов реального времени не работает." }, { "En", "The real-time clock battery has run out of power." } });
            errors.Add("UpdateNoAckSpace", new Dictionary<string, string> { { "Ru", "Подтвердждающие данные, запрошенные параметром обновления ack_data, равны NULL." }, { "En", "Acknowledge data that was requested by the update ack_data parameter is NULL." } });
            errors.Add("TerminalServiceDetected", new Dictionary<string, string> { { "Ru", "Обнаружена служба терминального доступа." }, { "En", "Program is running on a terminal server." } });
            errors.Add("FeatureNoteImplemented", new Dictionary<string, string> { { "Ru", "Запрошенный тип функции не реализован." }, { "En", "Requested Feature type is not implemented." } });
            errors.Add("UnknownAlgorithm", new Dictionary<string, string> { { "Ru", "Неизвестный алгоритм, используемый в файлах V2C или V2CP." }, { "En", "Unknown algorithm used in V2C or V2CP file." } });
            errors.Add("InvalidSignature", new Dictionary<string, string> { { "Ru", "Ошибка проверки подписи." }, { "En", "Signature verification operation failed." } });
            errors.Add("FeatureNotFound", new Dictionary<string, string> { { "Ru", "Запрошенная лицензия не найдена." }, { "En", "Requested Feature not found." } });
            errors.Add("NoLog", new Dictionary<string, string> { { "Ru", "Журнал доступа не включен." }, { "En", "Access log not enabled." } });
            errors.Add("LocalComErr", new Dictionary<string, string> { { "Ru", "Ошибка связи между API и локальным менеджером лицензий Sentinel." }, { "En", "Communication error occurred between the API and the local Sentinel License Manager." } });
            errors.Add("UnknownVcode", new Dictionary<string, string> { { "Ru", "Код поставщика не распознается API." }, { "En", "Vendor Code is not recognized by API." } });
            errors.Add("InvalidXmlSpec", new Dictionary<string, string> { { "Ru", "Недопустимая спецификация XML." }, { "En", "Invalid XML specification exists." } });
            errors.Add("InvalidXmlScope", new Dictionary<string, string> { { "Ru", "Недопустимая область XML." }, { "En", "Invalid XML scope exists." } });
            errors.Add("TooManyKeys", new Dictionary<string, string> { { "Ru", "Слишком много ключей защиты Sentinel в настоящее время подключены." }, { "En", "Too many Sentinel protection keys are currently connected." } });
            errors.Add("TooManyUsers", new Dictionary<string, string> { { "Ru", "В настоящее время подключено слишком много пользователей." }, { "En", "Too many users are currently connected." } });
            errors.Add("BrokenSession", new Dictionary<string, string> { { "Ru", "Сессия была прервана." + Environment.NewLine + "Это может произходить, например когда:" + Environment.NewLine + "   - Функция, необходимая для сеанса, была удалена." + Environment.NewLine + "    - Лицензия была отменена." + Environment.NewLine + "    - Изменена настройка поддержки сети (удаленной лицензии) для требуемой функции. (В этом случае все сеансы будут прерваны, включая локальные сеансы.)" }, { "En", "Session was interrupted. This can occur when certain updates are applied to the license while a session is active.For example:" + Environment.NewLine + "   - A Feature required by the session was deleted." + Environment.NewLine + "   - The license was canceled." + Environment.NewLine + "   - The network(remote license) support setting for a required Feature was changed. (In this case, all sessions will be interrupted, including local sessions.)" } });
            errors.Add("RemoteCommErr", new Dictionary<string, string> { { "Ru", "Ошибка связи между локальными и удаленными менеджерами лицензий Sentinel." }, { "En", "Communication error occurred between local and remote Sentinel License Managers." } });
            errors.Add("FeatureExpired", new Dictionary<string, string> { { "Ru", "Срок действия лицензии истек." }, { "En", "Feature expired or no executions remain." } });
            errors.Add("TooOldLM", new Dictionary<string, string> { { "Ru", "Версия менеджера лицензий Sentinel слишком старая." }, { "En", "Sentinel License Manager version too old." } });
            errors.Add("DeviceError", new Dictionary<string, string> { { "Ru", "Для ключа Sentinel SL произошла ошибка ввода / вывода в области безопасного хранилища." + Environment.NewLine + "ИЛИ" + Environment.NewLine + "Для ключа Sentinel HL произошла ошибка связи USB." }, { "En", "For a Sentinel SL key, an input/output error occurred in the secure storage area." + Environment.NewLine + "OR" + Environment.NewLine + "For a Sentinel HL key, a USB communication error occurred." } });
            errors.Add("UpdateBlocked", new Dictionary<string, string> { { "Ru", "Установка обновления запрещена." }, { "En", "Update installation not permitted." } });
            errors.Add("TimeError", new Dictionary<string, string> { { "Ru", "Системное время было изменено." }, { "En", "System time has been tampered with." } });
            errors.Add("SecureChannelError", new Dictionary<string, string> { { "Ru", "Ошибка связи в защищенном канале." }, { "En", "Communication error occurred in the secure channel." } });
            errors.Add("CorruptStorage", new Dictionary<string, string> { { "Ru", "Битые данные существуют в защищенной области хранения ключа защиты Sentinel." }, { "En", "Corrupt data exists in secure storage area of Sentinel protection key." } });
            errors.Add("VendorLibNotFound", new Dictionary<string, string> { { "Ru", "Невозможно найти кастомизированную библиотеку поставщика (haspvlib.vendorID.*)." }, { "En", "The customized vendor library (haspvlib.vendorID.*) cannot be located." } });
            errors.Add("InvalidVendorLib", new Dictionary<string, string> { { "Ru", "Не удалось загрузить библиотеку поставщика." }, { "En", "Unable to load Vendor library." } });
            errors.Add("EmptyScopeResults", new Dictionary<string, string> { { "Ru", "Невозможно найти лицензии, которая соответствует фильтру." }, { "En", "Unable to locate any Feature that matches the scope." } });
            errors.Add("VMDetected", new Dictionary<string, string> { { "Ru", "Защищенное приложение запускается на виртуальной машине, но для виртуальных машин не задействованы один или несколько функций." + Environment.NewLine + "ИЛИ" + Environment.NewLine + "Пользователь попытался повторно установить ключ защиты с физического компьютера на виртуальную машину. Однако ни один из функций, содержащихся в защитном ключе, не включен для виртуальных машин." }, { "En", "Protected application is running on a virtual machine, but one or more Features are not enabled for virtual machines." + Environment.NewLine + "OR" + Environment.NewLine + "The user attempted to rehost a protection key from a physical machine to a virtual machine. However, none of the Features contained in the protection key are enabled for virtual machines." } });
            errors.Add("HardwareModified", new Dictionary<string, string> { { "Ru", "Sentinel SL ключ несовместим с аппаратным оборудованием. Sentinel SL заблокирован для разных аппаратных средств." + Environment.NewLine + "ИЛИ" + Environment.NewLine + "В случае файла V2C или V2CP возникает конфликт между данными ключа Sentinel SL и данными аппаратного оборудования. Sentinel SL заблокирован для разных аппаратных средств." }, { "En", "Sentinel SL key incompatible with machine hardware. Sentinel SL key locked to different hardware." + Environment.NewLine + "OR" + Environment.NewLine + "In the case of a V2C or V2CP file, conflict between Sentinel SL key data and machine hardware data. Sentinel SL key locked to different hardware." } });
            errors.Add("UserDenied", new Dictionary<string, string> { { "Ru", "Вход запрещен из-за ограничений пользователя." }, { "En", "Login denied because of user restrictions." } });
            errors.Add("UpdateTooOld", new Dictionary<string, string> { { "Ru", "Попытка установить файл V2C или V2CP с помощью счетчика обновлений, который не соответствует порядку обновления с помощью ключа защиты Sentinel. Значения счетчика обновлений в файле ниже, чем в защитном ключе Sentinel." }, { "En", "Trying to install a V2C or V2CP file with an update counter that is out of sequence with update counter in the Sentinel protection key. Values of update counter in file are lower than those in Sentinel protection key." } });
            errors.Add("UpdateTooNew", new Dictionary<string, string> { { "Ru", "Попытка установить файл V2C или V2CP с помощью счетчика обновлений, который вышел из строя с помощью счетчика обновлений в защитном ключе Sentinel. Первое значение в файле больше, чем значение, в значении ключа защиты Sentinel." }, { "En", "Trying to install a V2C or V2CP file with an update counter that is out of sequence with the update counter in the Sentinel protection key. First value in file is more-than-1 greater than value in Sentinel protection key." } });
            errors.Add("VendorLibOld", new Dictionary<string, string> { { "Ru", "Библиотека поставщика слишком старая." }, { "En", "Vendor library is too old." } });
            errors.Add("UploadError", new Dictionary<string, string> { { "Ru", "Ошибка регистрации файла (например, V2C, H2R) с использованием Центра управления администратором, возможно из-за нелегального формата." }, { "En", "Check in of a file (such as  V2C, H2R) using Admin Control Center failed, possibly because of illegal format." } });
            errors.Add("InvalidRecipient", new Dictionary<string, string> { { "Ru", "Параметр RECIPIENT недействителен в XML." }, { "En", "Invalid XML RECIPIENT parameter." } });
            errors.Add("InvalidDetachAction", new Dictionary<string, string> { { "Ru", "Недопустимый параметр действия в XML." }, { "En", "Invalid XML action parameter." } });
            errors.Add("TooManyProducts", new Dictionary<string, string> { { "Ru", "Область, указанная в методе переноса, не указывает уникальный продукт." }, { "En", "The scope specified in the Transfer method does not specify a unique Product." } });
            errors.Add("InvalidProduct", new Dictionary<string, string> { { "Ru", "Неверная информация о продукте." }, { "En", "Invalid Product information." } });
            errors.Add("UnknownRecipient", new Dictionary<string, string> { { "Ru", "Обновление может применяться только к машине-получателю, указанной в методе отсоединения, а не к этой машине." }, { "En", "Update can only be applied to recipient machine specified in the Detach method, not to this machine." } });
            errors.Add("InvalidDuration", new Dictionary<string, string> { { "Ru", "Указан недопустимый период продолжительности отдельной лицензии. Продолжительность должна быть меньше или равна максимально допустимой для этой лицензии." + Environment.NewLine + "ИЛИ" + Environment.NewLine + "Продление продолжительности - до даты, предшествующей дате истечения текущей отдельной лицензии." }, { "En", "Invalid detached license duration period specified. Duration must be less than or equal to maximum allowed for this license." + Environment.NewLine + "OR" + Environment.NewLine + "Duration extension is to a date earlier than the expiration date of the current detached license." } });
            errors.Add("CloneDetected", new Dictionary<string, string> { { "Ru", "Было обнаружено клонированное хранилище Sentinel SL. Функция недоступна." }, { "En", "Cloned Sentinel SL storage was detected. Feature is unavailable." } });
            errors.Add("UpdateAlreadyAdded", new Dictionary<string, string> { { "Ru", "Указанное обновление V2C или или V2CP было уже установлено в службе менеджера лицензий." }, { "En", "The specified V2C or or V2CP update was already installed in the License Manager service." } });
            errors.Add("HaspInactive", new Dictionary<string, string> { { "Ru", "Указанный идентификатор ключа находится в неактивном состоянии." }, { "En", "Specified Key ID is in Inactive state." } });
            errors.Add("NoDetachableFeature", new Dictionary<string, string> { { "Ru", "В указанном ключе, от которого запрашивается отдельная лицензия, не существует съемной функции." }, { "En", "No detachable Feature exists in the specified key from which the detached license is requested." } });
            errors.Add("TooManyHosts", new Dictionary<string, string> { { "Ru", "Указанная область не определяет уникальный хост." }, { "En", "The specified scope does not specify a unique host." } });
            errors.Add("RehostNotAllowed", new Dictionary<string, string> { { "Ru", "Действие Rehost не разрешено для указанного идентификатора ключа." }, { "En", "Rehost action is not allowed for the specified Key  ID." } });
            errors.Add("LicenseRehosted", new Dictionary<string, string> { { "Ru", "Оригинальная лицензия была перенесена на другую машину. Поэтому лицензия не может быть возвращена исходной машине." }, { "En", "Original license has been transferred to another machine. Therefore, the license cannot be returned to the source machine." } });
            errors.Add("RehostAlreadyApplied", new Dictionary<string, string> { { "Ru", "Старая лицензия на восстановление не может применяться. Произошло несоответствие регресса-счетчика." }, { "En", "Old rehost license cannot be applied. A rehost-counter mismatch occurred." } });
            errors.Add("CannotReadFile", new Dictionary<string, string> { { "Ru", "Файл V2C или V2CP не найден или доступ запрещен." }, { "En", "A V2C or V2CP file was not found, or access was denied." } });
            errors.Add("ExtensionNotAllowed", new Dictionary<string, string> { { "Ru", "Лицензия не может быть расширена, поскольку количество отдельных лицензий больше, чем количество разрешенных одновременно разрешенных лицензий." }, { "En", "The license cannot be extended because the number of detached licenses is greater than the number of concurrent licenses allowed." } });
            errors.Add("DetachDisabled", new Dictionary<string, string> { { "Ru", "Пользователь попытался отключить продукт из сетевой лицензии, размещенной на виртуальной машине. Однако ни один из функций, включенных в Продукт, не включен для виртуальных машин." }, { "En", "The user attempted to detach a Product from a network license hosted on a virtual machine. However, none of the Features included in the Product are enabled for virtual machines." } });
            errors.Add("RehostDisabled", new Dictionary<string, string> { { "Ru", "Пользователь попытался восстановить ключ защиты с виртуальной машины. Однако ни один из функций, содержащихся в защитном ключе, не включен для виртуальных машин." }, { "En", "The user attempted to rehost a protection key from a virtual machine. However, none of the Features contained in the protection key are enabled for virtual machines." } });
            errors.Add("DetachedLicenseFound", new Dictionary<string, string> { { "Ru", "Пользователь попытался отформатировать ключ SL-AdminMode или перенести ключ SL-Legacy на ключ SL-AdminMode. Тем не менее, Продукт в настоящее время отключен от ключа." }, { "En", "The user attempted to format an SL-AdminMode key or to migrate an SL-Legacy key to an SL-AdminMode key. However, a Product is currently detached from the key." } });
            errors.Add("RecipientOldLm", new Dictionary<string, string> { { "Ru", "Для операции повторного использования: отпечаток целевой машины был собран с использованием инструментов (утилита RUS или API лицензирования) ранее, чем Sentinel LDK v.7.0." }, { "En", "For a rehost operation: The fingerprint of the target machine was collected using tools (RUS utility or Licensing API) earlier than Sentinel LDK v.7.0." } });
            errors.Add("SecureStoreIdMismatch", new Dictionary<string, string> { { "Ru", "Произошло несоответствие идентификатора безопасности." }, { "En", "A secure storage ID mismatch occurred." } });
            errors.Add("Duplicatehostname", new Dictionary<string, string> { { "Ru", "Отпечаток лицензии привязан к определенному имени хоста; однако в сети были обнаружены две или более машины с этим именем хоста. В результате лицензия не может быть использована." }, { "En", "The license fingerprint is bound to a specific hostname; however, two or more machines with this hostname were found in the network. As a result, the license cannot be used." } });
            errors.Add("MissingLM", new Dictionary<string, string> { { "Ru", "Защищенное приложение попыталось войти в функцию, поддерживающую параллелизм в Sentinel HL (Driverless configuration). Служба лицензирования лицензии Sentinel LDK неактивна на компьютере, где находится ключ." }, { "En", "	A protected application tried to log in to a Feature that supports concurrency on a Sentinel HL (Driverless configuration) key. The Sentinel LDK License Manager service is not active on the computer where the key is located." } });
            errors.Add("FeatureInsufficientExecutionCount", new Dictionary<string, string> { { "Ru", "Защищенное приложение пыталось использовать несколько исполнений при входе в функцию. Однако лицензия не содержит количество запрошенных исправлений." }, { "En", "A protected application tried to consume multiple executions while logging in to a Feature. However, the license does not contain the number of executions that were requested." } });
            errors.Add("HaspDisabled", new Dictionary<string, string> { { "Ru", "Ключ Sentinel HL (Driverless configuration) был отключен, поскольку пользователь попытался вмешаться в действие с помощью ключа или защищенного приложения." }, { "En", "A Sentinel HL (Driverless configuration) key was disabled because a user attempted to tamper with the key or with the protected application." } });
            errors.Add("NoApiDylib", new Dictionary<string, string> { { "Ru", "Невозможно найти динамическую библиотеку для API." }, { "En", "Unable to locate dynamic library for API." } });
            errors.Add("InvApiDylib", new Dictionary<string, string> { { "Ru", "Недопустимая динамическая библиотека для API." }, { "En", "Dynamic library for API is invalid." } });
            errors.Add("InvalidObject", new Dictionary<string, string> { { "Ru", "Объект был неправильно инициализирован." }, { "En", "Object was incorrectly initialized." } });
            errors.Add("InvalidParameter", new Dictionary<string, string> { { "Ru", "Строка области слишком длинная (максимальная длина 32 КБ)." }, { "En", "Scope string is too long (maximum length is 32 KB)." } });
            errors.Add("AreadyLoggedIn", new Dictionary<string, string> { { "Ru", "Вход дважды в тот же объект." }, { "En", "Logging in twice to same object." } });
            errors.Add("AlreadyLoggedOut", new Dictionary<string, string> { { "Ru", "Выйти дважды из того же объекта." }, { "En", "Logging out twice from same object." } });
            errors.Add("OperationFailed", new Dictionary<string, string> { { "Ru", "Неправильное использование системы или платформы." }, { "En", "Incorrect use of system or platform." } });
            errors.Add("HaspDotNetDllBroken", new Dictionary<string, string> { { "Ru", ".NET DLL найдена сломанной." }, { "En", ".NET DLL found broken." } });
            errors.Add("NotImplemented", new Dictionary<string, string> { { "Ru", "Запрошенная функция не была реализована." + Environment.NewLine + "ИЛИ" + Environment.NewLine + "В случае API-диспетчера API-библиотека DLL слишком старая." }, { "En", "Requested function was not implemented." + Environment.NewLine + "OR" + Environment.NewLine + "In the case of the API Dispatcher, API DLL is too old" } });
            errors.Add("InternalError", new Dictionary<string, string> { { "Ru", "Внутренняя ошибка в API." }, { "En", "Internal error occurred in the API." } });
        }

        public string ErrorMessageReplacer(string locale, string originalError)
        {
            string newErrorMessage = "";

            foreach (var el in SentinelData.errors) {
                if (originalError.Contains(el.Key)) {
                    newErrorMessage = (el.Value.ContainsKey(locale)) ? el.Value[locale] : el.Value["En"];
                    return newErrorMessage;
                }
            }

            if (String.IsNullOrEmpty(newErrorMessage)) newErrorMessage = originalError;

            return newErrorMessage;
        }
    }
}
