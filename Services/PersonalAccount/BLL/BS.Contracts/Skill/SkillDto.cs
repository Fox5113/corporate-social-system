using BS.Contracts.Base;
namespace BS.Contracts.Skill
{
    public class SkillDto
    {
        public Guid Id { get; set; }
        public TypeSkill Type { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
    }
}
