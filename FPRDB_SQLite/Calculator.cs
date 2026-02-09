using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPRDB_SQLite
{
    public class Calculator
    {
        // Hàm cộng hai số
        public int Add(int a, int b)
        {
            return a + b;
        }

        // Hàm kiểm tra số chẵn
        public bool IsEven(int number)
        {
            return number % 2 == 0;
        }

        // Hàm chia (để test trường hợp lỗi chia cho 0)
        public double Divide(int a, int b)
        {
            if (b == 0)
            {
                throw new DivideByZeroException("Không thể chia cho số 0");
            }
            return (double)a / b;
        }
    }
}
