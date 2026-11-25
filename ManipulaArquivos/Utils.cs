internal static class Utils
{
    internal static char CreateMenu(List<string> options)
    {
        foreach (string item in options)
            Console.Write(item);
        try
        {
            Console.Write("Informe a opção desejada: ");
            return char.TryParse(Console.ReadLine(), out char option) ? option : '\0';
        }
        catch
        {
            return '\0';
        }
    }

    internal static bool ValidateFilePath(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            Console.WriteLine("O caminho do arquivo não pode ser vazio.");
            return false;
        }
        if (!File.Exists(filePath))
        {
            Console.WriteLine("O arquivo especificado não existe.");
            return false;
        }
        return true;
    }

    internal static List<Contact> LoadContacts(string filePath)
    {
        var contacts = new List<Contact>();
        try
        {
            if (Utils.ValidateFilePath(filePath))
                foreach (var contact in File.ReadAllLines(filePath))
                {
                    var contactData = contact.Split(';');
                    if (contactData.Length.Equals(3))
                    {
                        var name = contactData[0];
                        var phone = contactData[1];
                        var gender = Enum.TryParse<EGender>(contactData[2],
                            out var parsedGender) ? parsedGender : EGender.NaoInformado;
                        var newContact = new Contact(name, phone, gender);
                        contacts.Add(newContact);
                    }
                }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Ocorreu um erro ao carregar os contatos: {e.Message}");
        }
        return contacts;
    }

    internal static void SaveContacts(List<Contact> contacts, string filePath)
    {
        try
        {
            var directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            using (var sw = new StreamWriter(filePath))
            {
                foreach (var contact in contacts)
                {
                    sw.WriteLine(contact.ToFileString());
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Ocorreu um erro ao salvar os contatos: {e.Message}");
        }
    }
}
