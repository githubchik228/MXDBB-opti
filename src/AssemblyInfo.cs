using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

// Метаданные версии → попадают в свойства .exe (вкладка «Подробно»):
// Издатель, Продукт, Описание, Версия, Авторские права.
// Пустые метаданные + права админа + запись в реестр = высокий «подозрительный»
// балл у эвристики Defender. Заполненные метаданные снижают ложные срабатывания.

[assembly: AssemblyTitle("MXDBB Opti")]
[assembly: AssemblyProduct("MXDBB Opti")]
[assembly: AssemblyDescription("Оптимизатор игрового ПК — безопасные обратимые твики Windows")]
[assembly: AssemblyCompany("MXDBB Opti")]
[assembly: AssemblyCopyright("© 2026 MXDBB Opti · github.com/githubchik228/MXDBB-opti")]
[assembly: AssemblyTrademark("MXDBB Opti")]
[assembly: AssemblyConfiguration("")]
[assembly: NeutralResourcesLanguage("ru-RU")]

[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: AssemblyInformationalVersion("1.0")]

[assembly: ComVisible(false)]
