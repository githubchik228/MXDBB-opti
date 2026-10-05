# MXDBB Opti

MXDBB Opti — локальный Windows gaming-PC optimizer на базе оригинального FPS-TOOL engine, переработанный в самостоятельный инструмент.

## Что есть
- One-click оптимизация Windows с обратимыми твиками
- Точка восстановления + экспорт реестра перед применением
- Откат выбранных оптимизаций и восстановление последнего бэкапа
- Game Mode / Game DVR / HAGS / power plan / network / UI responsiveness
- Глубокая очистка temp, браузеров, shader cache, WER, Windows cache и DNS
- Live Monitor: CPU, RAM, GPU/NVIDIA, VRAM, диск, сеть, ping и top processes
- Game Boost: запуск выбранной игры с High Performance plan + AboveNormal priority
- SFC / DISM / DNS инструменты восстановления
- Hardware scan
- CLI: selftest, dryrun, apply, revert
- Нет лицензий, подписок, телеметрии и фонового сервиса

## Важные принципы
- MXDBB не отключает Defender или Windows Update по умолчанию.
- BIOS автоматически не прошивается и не изменяется.
- Real-time priority и опасные системные настройки не используются.
- Если датчик GPU недоступен, программа не подставляет выдуманные значения.
- Перед применением твиков создаётся restore point и .reg backup.

## Build
На Windows:
1. Установи .NET Framework 4.x Developer/Runtime с компилятором csc.exe.
2. Запусти src\build.cmd.
3. Получишь src\MXDBB Opti.exe.

GitHub Actions автоматически собирает проект на Windows при push/PR.
