using System.Text;

string? currentFile = "Не указан";
string message = "";
StringBuilder buffer = new StringBuilder();

while (true)
{
    ShowMenu();
}

void ShowMenu()
{
    Console.Clear();
    Console.WriteLine($"Текущий файл: {currentFile}");
    Console.WriteLine("----------------------------------------------------------------");

    Console.WriteLine(message);
    message = "";
    Console.WriteLine("----------------------------------------------------------------");

    Console.WriteLine($"0. Ввести текст\n1. Создать файл\n2. Выбрать файл\n3. Сохранить\n4. Сохранить как");
    Console.Write("Выберите действие: ");
    var res = int.TryParse(Console.ReadLine(), out int number);
    if (number == 0) TextInput();
    if (number == 1) if (CreateFile(out string? path, out message)) currentFile = path;
    if (number == 2) if (SelectFile(out string? path, out message)) currentFile = path;
    if (number == 3) SaveFile(out message);
    if (number == 4) SaveFile(out message, true);
}

void TextInput()
{
    Console.Clear();
    Console.WriteLine("Для завершения ввода введите !Q");
    Console.WriteLine("------------------------------------------");
    using (StreamReader sr = new StreamReader(currentFile))
    {
        Console.WriteLine(sr.ReadToEnd());
    }
    Console.Write(buffer.ToString());
    while (true)
    {
        var text = Console.ReadLine();
        if (text.Equals("!Q")) break;
        buffer.Append($"{text}\n");
    }
}

bool SaveFile(out string message, bool saveAs = false)
{
    message = "";
    bool canSave = true;
    string? path;
    if (!saveAs) path = currentFile;
    else path = "Не указан";
    if (path.Equals("Не указан"))
    {
        if (CreateFile(out path, out message)) canSave = true;
        else canSave = false;
    }

    if (canSave)
    {
        using StreamWriter sw = new StreamWriter(path, true);
        sw.Write(buffer.ToString());
        buffer.Clear();
        message = $"Файл {path} сохранен";
        currentFile = path;
        return true;
    }
    else
    {
        return false;
    }
}


bool SelectFile(out string? path, out string message)
{
    Console.Clear();
    message = "Файл не выбран";
    path = "Не указан";
    string[] files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.txt");
    for (int i = 0; i < files.Length; i++)
    {
        FileInfo fileInfo = new FileInfo(files[i]);
        Console.WriteLine($"{i}. {fileInfo.Name}");
    }
    Console.Write("Введите номер файла:");
    var sel = int.TryParse(Console.ReadLine(), out int number);
    if (sel)
    {
        if (number < files.Length && number >= 0)
        {
            FileInfo fi = new FileInfo(files[number]);
            path = fi.Name;
            message = $"Выбран файл {path}";
            return true;
        }
        else return false;
    }
    else return false;
}

bool CreateFile(out string? path, out string message)
{
    bool canCreate = false;
    Console.Write("Введите имя файла (оставьте пустым для отмены): ");
    path = Console.ReadLine();
    if (path == null || path.Equals("")) canCreate = false;
    else if (File.Exists(path))
    {
        Console.Write($"Файл {path} уже существует. Перезаписать файл? Да(Y):");
        var res = Console.ReadLine();
        if (res == "Y") canCreate = true;
        else canCreate = false;
    }
    else canCreate = true;


    if (canCreate)
    {
        try
        {
            using FileStream fs = File.Create(path);
            message = $"Файл {path} создан";
            return true;
        }
        catch (Exception ex)
        {
            message = $"Ошибка при создании файла. {ex.Message}";
            return false;
        }
    }
    else { message = "Отмена создания файла"; return false; }
}

