namespace TemplateContainers
{
    public class BaseTemplate
    {
        public const int InvidateTemplateId = -1;
        public int Id { get; set; } = InvidateTemplateId;

        public string Name { get; set; }

        public bool Invalid()
        {
            return Id == -1;
        }
    }
}
