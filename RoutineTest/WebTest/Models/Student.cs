using System.ComponentModel.DataAnnotations;

namespace WebTest.Models
{

    public class Student
    {

        [Display(Name = "年龄", Order = 2)]
        public int? Age { get; set; }

        [Display(Name = "姓名", Order = 1)]
        [Required(ErrorMessage = "姓名不能为空")]
        [StringLength(5, MinimumLength = 2, ErrorMessage = "请输入正确的姓名")]
        public string Name { get; set; }
        public object child { get; set; }
    }

    public class Child
    {
        public string Cname { get; set; }
        public int Cage { get; set; }
    }

}