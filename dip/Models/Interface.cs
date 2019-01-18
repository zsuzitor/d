using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace dip.Models
{
    public class Interface
    {
       


    }

    public abstract class Item
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Parent { get; set; }


public static List<string> GetQueueParent<T>(List<T> list) where T: Item
        {
            List<T> withOutCHild = null;
            withOutCHild = list.Where(x1 => list.FirstOrDefault(x2 => x2.Parent == x1.Id) == null).ToList();
            List<string> prosPath = new List<string>();
            foreach (var i in withOutCHild)
            {
                string onePath = "";
                List<T> queue = new List<T>();
                queue.Add(i);

                // bool ex = false;
                string prID = i.Parent;



                while (true)
                {

                    var pr = list.FirstOrDefault(x1 => x1.Id == prID);
                    if (pr == null)
                        break;
                    queue.Add(pr);
                    prID = pr.Parent;


                }
                for (var n = queue.Count - 1; n >= 0; --n)
                {
                    onePath += queue[n].Name + "->";
                }
                prosPath.Add(onePath);
            }
            return prosPath;
        }


    }


    //public interface CheckBoxForm
    //{
    //    string Id { get; set; }
    //    string Name { get; set; }
    //    string Parent { get; set; }
    //}




}