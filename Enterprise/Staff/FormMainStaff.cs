using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Xml.Linq;
using System.Windows.Forms;
using Aladdin.HASP;
using MyLogClass;

namespace Staff
{
    public partial class FormMainStaff : Form
    {
        public static Dictionary<string, Dictionary<string, string>> errors = new Dictionary<string, Dictionary<string, string>>();
        public static bool lIsEnabled, aIsEnabled;
        public static bool logsIsExist = false, logsDirIsExist = false, logsFileIsExist = false;
        public static string language;
        public static string baseDir, logFileName;
        public static HaspFeature feature;
        public static string scope = "", format = "", info = "";
        public static string keyId = "";
        public static string vendorCode = "";
        public static Hasp hasp;
        public static HaspStatus status;
        public static MultiLanguage alp;

        public FormMainStaff(string[] args)
        {
            InitializeComponent();

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

            // внутренние ошибки
            errors.Add("Invalid ProductKey or C2V", new Dictionary<string, string> { { "Ru", "Некорректный ключ активации или C2V." + Environment.NewLine + "Пожалуйста проверьте и повторите попытку." }, { "En", "Invalid ProductKey or C2V." + Environment.NewLine + "Please check it and try again." } });
            errors.Add("Error in request C2V", new Dictionary<string, string> { { "Ru", "Ошибка запроса C2V." }, { "En", "Error in request C2V." } });
            errors.Add("Do you want to install license in New SL Key", new Dictionary<string, string> { { "Ru", "Вы хотите установить лицензии в виде нового SL ключа?" }, { "En", "Do you want to install license in New SL Key?" } });
            errors.Add("Do you want to install license in exist Key", new Dictionary<string, string> { { "Ru", "Хотите ли Вы установить лицензии в существующий ключ: {0} с Key ID = {1}? Если выберете \"Нет\", лицензии будут установлены в виде нового SL ключа." }, { "En", "Do you want to install license in exist Key: {0} with Key ID = {1}? If you chouse \"No\", license will be installed in new SL key." } });
            errors.Add("No pending update", new Dictionary<string, string> { { "Ru", "Нет доступных для загрузки обновлений." }, { "En", "No pending update." } });
            errors.Add("Response from server has error or empty", new Dictionary<string, string> { { "Ru", "Ответ от сервера пустой или содержит ошибку." }, { "En", "Response from server has error or empty." } });
            errors.Add("Error", new Dictionary<string, string> { { "Ru", "Ошибка" }, { "En", "Error" } });
            errors.Add("Warning", new Dictionary<string, string> { { "Ru", "Предупреждение" }, { "En", "Warning" } });
            errors.Add("License update successfully installed", new Dictionary<string, string> { { "Ru", "Лицензия применена успешно!" }, { "En", "License update successfully installed!" } });
            errors.Add("Update didn't installed", new Dictionary<string, string> { { "Ru", "Обновление не установлено!" }, { "En", "Update didn't installed!" } });
            errors.Add("Saving file error", new Dictionary<string, string> { { "Ru", "Ошибка при сохранении файла: {0}" }, { "En", "Saving file error: {0}" } });
            errors.Add("Error: Accounting.exe not found in dir", new Dictionary<string, string> { { "Ru", "Ошибка: Accounting.exe не найден в директории: {0}" }, { "En", "Error: Accounting.exe not found in dir: {0}" } });
            errors.Add("Error: Stock.exe not found in dir", new Dictionary<string, string> { { "Ru", "Ошибка: Stock.exe не найден в директории: {0}" }, { "En", "Error: Stock.exe not found in dir: {0}" } });
            errors.Add("Error: Staff.exe not found in dir", new Dictionary<string, string> { { "Ru", "Ошибка: Staff.exe не найден в директории: {0}" }, { "En", "Error: Staff.exe not found in dir: {0}" } });
            errors.Add("Error: ", new Dictionary<string, string> { { "Ru", "Ошибка: {0}" }, { "En", "Error: {0}" } });
            errors.Add("Can't create dir for logs", new Dictionary<string, string> { { "Ru", "Не получается создать директорию для логов! Ошибка: {0}" }, { "En", "Can't create dir for logs! Error: {0}" } });
            errors.Add("Can't create log file", new Dictionary<string, string> { { "Ru", "Не получается создать файл с логами! Ошибка: {0}" }, { "En", "Can't create log file! Error: {0}" } });
            errors.Add("File \"Enterprise.exe.config\" doesn't exist in dir", new Dictionary<string, string> { { "Ru", "Файл \"Enterprise.exe.config\" не существует в директории:\n {0}" }, { "En", "File \"Enterprise.exe.config\" doesn't exist in dir:\n {0}" } });
            //errors.Add("", new Dictionary<string, string> { { "Ru", "" }, { "En", "" } });

            alp = new MultiLanguage();

            // получение пути до базовой директории где расположено приложение
            //============================================= 
            System.Reflection.Assembly a = System.Reflection.Assembly.GetEntryAssembly();
            baseDir = System.IO.Path.GetDirectoryName(a.Location);
            //=============================================

            // создаём директорию (если не создана) и файл с логами
            //=============================================
            if (System.IO.Directory.Exists(baseDir + "\\logs")) { //если директория с логами есть, говорим true
                logsDirIsExist = true;
            } else { // если нет, пробуем создать и ещё раз проверяем создалась ли 
                try {
                    System.IO.Directory.CreateDirectory(baseDir + "\\logs");
                    logsDirIsExist = System.IO.Directory.Exists(baseDir + "\\logs");
                } catch (Exception ex) {
                    MessageBox.Show(ErrorMessageReplacer(language, "Can't create dir for logs").Replace("{0}", ex.Message));
                }
            }

            if (logsDirIsExist == true) { // если директория с логами есть, проверяем есть ли файл с логами если есть - используем его, если нет - создаём файл с логами 
                logFileName = "app.log";

                if (System.IO.File.Exists(baseDir + "\\logs\\" + logFileName)) { // если файл с логами есть, говорим true
                    logsFileIsExist = true;
                } else { // если нет, пробуем создать и ещё раз проверяем создался ли 
                    try {
                        using (System.IO.File.Create(baseDir + "\\logs\\" + logFileName)) {
                            logsFileIsExist = System.IO.Directory.Exists(baseDir + "\\logs");
                        }
                    } catch (Exception ex) {
                        MessageBox.Show(ErrorMessageReplacer(language, "Can't create log file").Replace("{0}", ex.Message));
                    }
                }
            }
            //=============================================

            if (lIsEnabled) Log.Write("Запускаем приложение Staff.exe");

            if (lIsEnabled) Log.Write("Разбираем настройки приложения, переданные при его вызове...");
            foreach (string srg in args) {
                string[] arg = srg.Split(':');
                switch (arg[0]) {
                    case "vcode":
                        vendorCode = arg[1];
                        break;

                    case "kid":
                        keyId = arg[1];
                        break;

                    case "fid":
                        feature = HaspFeature.FromFeature(Convert.ToInt32(arg[1]));
                        break;

                    case "api":
                        aIsEnabled = (arg[1] == "True") ? true : false;
                        break;

                    case "logs":
                        lIsEnabled = (arg[1] == "True") ? true : false;
                        break;

                    case "language":
                        language = arg[1];
                        break;
                }
            }

            if (args.Length < 1) {
                if (lIsEnabled) Log.Write("Запуск Staff.exe производился без требуемых параметров (предположительно не из Enterprise.exe)");
                if (lIsEnabled) Log.Write("Пробуем читать общий файл с конфигами приложений: Enterprise.exe.config");
                XDocument settingsXml = new XDocument();
                if (!File.Exists(baseDir + Path.DirectorySeparatorChar + "Enterprise.exe.config")) {
                    MessageBox.Show(ErrorMessageReplacer(language, "File \"Enterprise.exe.config\" doesn't exist in dir"), ErrorMessageReplacer(language, "Error"));
                    if (lIsEnabled) Log.Write("Ошибка, файл \"Enterprise.exe.config\" не найден в директории: " + baseDir);
                    Environment.Exit(0);
                } else {
                    settingsXml = XDocument.Load(baseDir + Path.DirectorySeparatorChar + "Enterprise.exe.config");
                }

                if (lIsEnabled) Log.Write("Парсим файл с конфигами: " + baseDir + Path.DirectorySeparatorChar + "Enterprise.exe.config");
                foreach (XElement el in settingsXml.Root.Elements()) {
                    foreach (XElement elEnterpriseSettingsEnterprise in el.Elements("Enterprise.settings.enterprise")) {
                        foreach (XElement elSetting in elEnterpriseSettingsEnterprise.Elements("setting")) {
                            switch (elSetting.Attribute("name").Value) {
                                case "enableLogs":
                                    lIsEnabled = (String.IsNullOrEmpty(elSetting.Value)) ? true : Convert.ToBoolean(elSetting.Value);
                                    break;

                                case "language":
                                    language = (String.IsNullOrEmpty(elSetting.Value)) ? "" : elSetting.Value;
                                    break;

                                case "vendorCode":
                                    string tmpVCode = "";
                                    if (elSetting.Value.Split(' ').Length > 1)
                                    {
                                        tmpVCode = elSetting.Value.Split(' ')[1];
                                    }
                                    else
                                    {
                                        tmpVCode = elSetting.Value.Split(' ')[0];
                                    }
                                    vendorCode = (String.IsNullOrEmpty(elSetting.Value)) ? "AzIceaqfA1hX5wS+M8cGnYh5ceevUnOZIzJBbXFD6dgf3tBkb9cvUF/Tkd/iKu2fsg9wAysYKw7RMAsVvIp4KcXle/v1RaXrLVnNBJ2H2DmrbUMOZbQUFXe698qmJsqNpLXRA367xpZ54i8kC5DTXwDhfxWTOZrBrh5sRKHcoVLumztIQjgWh37AzmSd1bLOfUGI0xjAL9zJWO3fRaeB0NS2KlmoKaVT5Y04zZEc06waU2r6AU2Dc4uipJqJmObqKM+tfNKAS0rZr5IudRiC7pUwnmtaHRe5fgSI8M7yvypvm+13Wm4Gwd4VnYiZvSxf8ImN3ZOG9wEzfyMIlH2+rKPUVHI+igsqla0Wd9m7ZUR9vFotj1uYV0OzG7hX0+huN2E/IdgLDjbiapj1e2fKHrMmGFaIvI6xzzJIQJF9GiRZ7+0jNFLKSyzX/K3JAyFrIPObfwM+y+zAgE1sWcZ1YnuBhICyRHBhaJDKIZL8MywrEfB2yF+R3k9wFG1oN48gSLyfrfEKuB/qgNp+BeTruWUk0AwRE9XVMUuRbjpxa4YA67SKunFEgFGgUfHBeHJTivvUl0u4Dki1UKAT973P+nXy2O0u239If/kRpNUVhMg8kpk7s8i6Arp7l/705/bLCx4kN5hHHSXIqkiG9tHdeNV8VYo5+72hgaCx3/uVoVLmtvxbOIvo120uTJbuLVTvT8KtsOlb3DxwUrwLzaEMoAQAFk6Q9bNipHxfkRQER4kR7IYTMzSoW5mxh3H9O8Ge5BqVeYMEW36q9wnOYfxOLNw6yQMf8f9sJN4KhZty02xm707S7VEfJJ1KNq7b5pP/3RjE0IKtB2gE6vAPRvRLzEohu0m7q1aUp8wAvSiqjZy7FLaTtLEApXYvLvz6PEJdj4TegCZugj7c8bIOEqLXmloZ6EgVnjQ7/ttys7VFITB3mazzFiyQuKf4J6+b/a/Y" : tmpVCode;
                                    break;

                                case "scope":
                                    string scopeTmp = (String.IsNullOrEmpty(elSetting.Value)) ? "<haspscope>" +
                                                                                                    "<feature>" +
                                                                                                        "<name>Staff</name>" +
                                                                                                        "<id>3</id>" +
                                                                                                    "</feature>" +
                                                                                                "</haspscope>" : elSetting.Value;
                                    XDocument scopeTmpXml = XDocument.Parse(scopeTmp);
                                    foreach (XElement elScope in scopeTmpXml.Root.Elements()) {
                                        foreach (XElement elFeatureName in elScope.Elements("name")) {
                                            if (elFeatureName.Value == "Staff") {
                                                foreach (XElement elFeatureId in elScope.Elements("id")) {
                                                    feature = HaspFeature.FromFeature(Convert.ToInt32(elFeatureId.Value));
                                                }
                                            }
                                        }
                                    }
                                    break;

                                case "enableApi":
                                    aIsEnabled = (String.IsNullOrEmpty(elSetting.Value)) ? true : Convert.ToBoolean(elSetting.Value);
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private void FormMainStaff_Load(object sender, EventArgs e)
        {
            if (lIsEnabled) Log.Write("Загружаем/применяем Language Pack к приложению");
            FormMainStaff mForm = (FormMainStaff)Application.OpenForms["FormMainStaff"];
            bool isSetAlpFormMain = alp.SetLenguage(language, baseDir + "\\language\\" + language + ".alp", this.Controls, mForm);

            if (aIsEnabled) {
                if (lIsEnabled) Log.Write("Использование API включено, пробуем получить Key ID с требуемой для работы лицензией");

                scope = "<haspscope>" +
                            "<feature id=\"" + feature.FeatureId.ToString() + "\"/>" +
                        "</haspscope>";

                format = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                         "<haspformat root=\"hasp_info\">" +
                             "<hasp>" +
                                 "<element name=\"id\"/>" +
                             "</hasp>" +
                         "</haspformat>";

                status = Hasp.GetInfo(scope, format, vendorCode, ref info);
                if (lIsEnabled) Log.Write("Результат выполнения функции GetInfo: " + status);

                if (HaspStatus.StatusOk != status) {
                    //handle error
                    MessageBox.Show(ErrorMessageReplacer(language, status.ToString()), ErrorMessageReplacer(language, "Error"));
                    if (lIsEnabled) Log.Write("Ключа с требуемой лицензией не найдено. Закрываем приложение Staff.exe.");
                    Environment.Exit(0);
                } else {
                    XDocument infoXml = XDocument.Parse(info);
                    foreach (XElement el in infoXml.Root.Elements()) {
                        keyId = el.Value;
                    }

                    if (lIsEnabled) Log.Write("Найден ключ с требуемой лицензией, Key ID ключа: " + keyId);

                    if (lIsEnabled) Log.Write("Выполняем запрос лицензии с ключа: " + keyId);
                    hasp = new Hasp(feature);

                    scope = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                            "<haspscope>" +
                                "<hasp id=\"" + keyId + "\"/>" +
                            "</haspscope>";

                    status = hasp.Login(vendorCode, scope);
                    if (lIsEnabled) Log.Write("Результат логина на лицензию в ключе: " + status);
                    if (HaspStatus.StatusOk != status) {
                        //handle error
                        MessageBox.Show(ErrorMessageReplacer(language, status.ToString()), ErrorMessageReplacer(language, "Error"));
                        if (lIsEnabled) Log.Write("Ошибка подключения к лицензии в ключе. Закрываем приложение Staff.exe.");
                        Environment.Exit(0);
                    } else {
                        //MessageBox.Show("Status: " + status, "Successfully");
                        if (lIsEnabled) Log.Write("Требуемая лицензия обнаружена. Продолжаем работу приложения.");
                    }
                }
            }
        }

        private void FormMainStaff_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (lIsEnabled) Log.Write("Закрываем приложение Staff.exe");
            if (aIsEnabled) {
                if (lIsEnabled) Log.Write("Использование API включено, требуется выполнить Logout перед закрытием приложения");

                status = hasp.Logout();
                if (lIsEnabled) Log.Write("Результат выполнения функции Logout: " + status);
                if (HaspStatus.StatusOk != status) {
                    //handle error
                    //MessageBox.Show("Error: " + status, "Error");
                    if (lIsEnabled) Log.Write("Ошибка при выполнении функции Logout. Всё равно закрываем приложение.");
                } else {
                    //MessageBox.Show("Status: " + status, "Successfully");
                    if (lIsEnabled) Log.Write("Logout выполнен успешно, закрываем приложение.");
                }
            }
        }

        public string ErrorMessageReplacer(string locale, string originalError)
        {
            string newErrorMessage = "";

            foreach (var el in errors)
            {
                if (originalError.Contains(el.Key))
                {
                    newErrorMessage = (el.Value.ContainsKey(locale)) ? el.Value[locale] : el.Value["En"];
                    return newErrorMessage;
                }
            }

            if (String.IsNullOrEmpty(newErrorMessage)) newErrorMessage = originalError;

            return newErrorMessage;
        }
    }
}
