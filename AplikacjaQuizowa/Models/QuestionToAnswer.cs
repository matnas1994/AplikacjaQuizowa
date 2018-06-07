using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AplikacjaQuizowa.Models
{
    public class QuestionToAnswer
    {
        public int QuestionId { get; set; }
  
        public string Contents { get; set; }

        public List<Answer> Answer { get; set; }   

        public QuestionToAnswer()
        {
            Answer = new List<Answer>();
        }

        public void mixingAnswer() {
 
            int n = Answer.Count;
            Random rnd = new Random(DateTime.Now.Millisecond-QuestionId);
            while (n > 1)
            {
                int k = (rnd.Next(0, n) % n);
                n--;
                Answer value = Answer[k];
                Answer[k] = Answer[n];
                Answer[n] = value;
            }

            
        }
    }
}