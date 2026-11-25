public class Contact(string name, string phone, EGender gender)
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; } = name;
    public string Phone { get; private set; } = phone;
    public EGender? Gender { get; private set; } = gender;

    public override string ToString()
    {
        return $"Id: {Id.ToString().Split('-').Last()}\n" +
            $"Nome: {Name}\n" +
            $"Telefone: {Phone}\n" +
            $"Gênero: {Gender}";
    }

    public string ToFileString()
    {
        return $"{Name};{Phone};{Gender}";
    }
}
