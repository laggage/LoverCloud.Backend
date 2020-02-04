namespace LoverCloud.Infrastructure.Services
{
    public class MappedProperty
    {
        public MappedProperty(string name, bool revert = false)
        {
            Name = name;
            Revert = revert;
        }

        public string Name { get; set; }
        /// <summary>
        /// 用来指示目标和源之间是否为反义; 比如, 目标属性的升序对应到源属性可能就是降序
        /// </summary>
        public bool Revert { get; set; }
    }
}
