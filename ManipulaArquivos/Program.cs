var options = new List<string>(){
    "1 - Criar Contato\n",
    "2 - Ler Contatos\n",
    "3 - Deletar Contato\n",
    "4 - Salvar Agenda\n",
    "0 - Sair\n"
};

var saveControl = false;

var filePath = Environment.GetEnvironmentVariable("USERPROFILE") + @"\Agenda\contacts.db";

var contacts = Utils.LoadContacts(filePath);

while (true)
{
    Console.Clear();
    Console.WriteLine(filePath);
    switch (Utils.CreateMenu(options))
    {
        case '1':
            try
            {
                if (CreateContact() is Contact contact && contact is not null)
                {
                    contacts.Add(contact);
                    Console.WriteLine($"Contato {contact.Id} adicionado com sucesso!");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadKey();
            }
            break;

        case '2':
            ShowContacts(contacts);
            break;

        case '3':
            Console.Write("Informe o nome do contato que deseja remover:");
            var c = SearchByName(Console.ReadLine() ?? string.Empty);

            if (c is not null)
            {
                contacts.Remove(c);
                Console.WriteLine($"Contato {c.Name} removido com sucesso!");
            }
            else
            {
                Console.WriteLine("Contato não encontrado!");
                Console.ReadKey();
            }
            break;

        case '4':
            Utils.SaveContacts(contacts, filePath);
            Console.WriteLine("Agenda salva com sucesso!");
            Thread.Sleep(1000);
            saveControl = !saveControl;
            break;

        case '0':
            if (!saveControl)
            {
                Console.Write("Deseja salvar a agenda antes de sair? (S/N): ");
                var saveOption = Console.ReadLine();
                if (saveOption?.ToUpper() == "S")
                {
                    Utils.SaveContacts(contacts, filePath);
                    Console.WriteLine("Agenda salva com sucesso!");
                    Thread.Sleep(1000);
                }
            }
            Environment.Exit(0);
            break;

        default:
            Console.WriteLine("Opção inválida!");
            Thread.Sleep(1000);
            break;
    }
}

Contact? SearchByName(string name)
{
    if (name is null)
        return null;

    return contacts.FirstOrDefault(c =>
    c.Name.Equals(name,
    StringComparison.OrdinalIgnoreCase));
}

void ShowContacts(List<Contact> contacts)
{
    if (contacts.Count.Equals(0))
    {
        Console.WriteLine("Nenhum contato encontrado!");
        Console.ReadKey();
        return;
    }

    int index = 0;
    bool showing = true;

    while (showing)
    {
        var options = new List<string> { "1 - Primeiro ", "2 - Anterior ", "3 - Próximo ", "4 - Último ", "0 - Sair\n" };

        Console.Clear();
        Console.WriteLine($"Item {index + 1} de {contacts.Count}");
        Console.WriteLine(contacts[index] + "\n");

        switch (Utils.CreateMenu(options))
        {
            case '1':
                index = 0;
                break;

            case '2':
                if (index > 0) index--;
                break;

            case '3':
                if (index < contacts.Count - 1) index++;
                break;

            case '4':
                index = contacts.Count - 1;
                break;

            case '0':
                showing = false;
                break;

            default:
                Console.WriteLine("Opção inválida!");
                break;
        }
        //Console.ReadKey();
        //Console.ReadKey();
    }
}

Contact? CreateContact()
{
    Contact contact = null;
    try
    {
        string name = string.Empty;
        string phone = string.Empty;

        while (string.IsNullOrEmpty(name))
        {
            Console.Write("Informe o nome do contato: ");
            name = Console.ReadLine();
        }

        Console.Write("Informe o telefone do contato: ");
        while (string.IsNullOrEmpty(phone))
        {
            phone = Console.ReadLine();
        }

        foreach (var item in Enum.GetValues(typeof(EGender)))
        {
            Console.WriteLine($"{(int)item} - {item}");
        }

        Console.Write("Informe o gênero do contato: ");

        var gender = int.TryParse(Console.ReadLine(), out int value) &&
            Enum.IsDefined(typeof(EGender), value)
        ? (EGender)value
        : EGender.NaoInformado;

        contact = new Contact(name, phone, gender);
    }
    catch
    {
        throw new Exception("Ocorreu um erro ao criar o contato.");
    }

    return contact;
}