using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GeneratorProgowy
{

    public partial class MainWindow : Window
    {
        private int howMuchRegisters = 3,
            maximumLengthOfRegister = 20,
            keyLength = 50000;

        private static Random random = new Random();
        private List<int> registersLengths = new List<int>();
        private List<KeyValuePair<List<String>, List<String>>> registers = new List<KeyValuePair<List<String>, List<String>>>();
        private List<KeyValuePair<List<String>, List<String>>> registersBackup = new List<KeyValuePair<List<String>, List<String>>>();

        public MainWindow()
        {
            InitializeComponent();   
        }
        #region Losowanie
        public static List<int> GenerateRandom(int count, int min, int max)
        {
            if (max <= min || count < 0 ||

                    (count > max - min && max - min > 0))
            {

                throw new ArgumentOutOfRangeException("Range " + min + " to " + max +
                        " (" + ((Int64)max - (Int64)min) + " values), or count " + count + " is illegal");
            }


            HashSet<int> candidates = new HashSet<int>();

            for (int top = max - count; top < max; top++)
            {
                if (!candidates.Add(random.Next(min, top + 1)))
                {

                    candidates.Add(top);
                }
            }


            List<int> result = candidates.ToList();
            for (int i = result.Count - 1; i > 0; i--)
            {
                int k = random.Next(i + 1);
                int tmp = result[k];
                result[k] = result[i];
                result[i] = tmp;
            }
            return result;
        }
        #endregion

        #region Utworzenie rejestru
        private KeyValuePair<List<String>, List<String>> CreateRegister()
        {
            List<String> registerContent = new List<String>();
            List<String> polynomial = new List<String>();
            polynomial.Add("1");
            int randomNumber = -1;
            do
            {
                randomNumber = GenerateRandom(1, 1, maximumLengthOfRegister+1)[0];
            } while (registersLengths.Contains(randomNumber));
            registersLengths.Add(randomNumber);

            for (int i = 0; i < randomNumber; i++)
            {
                polynomial.Add(GenerateRandom(1, 0, 2)[0].ToString());
            }
            for(int  i = 0; i<randomNumber+2;i++)
                registerContent.Add(GenerateRandom(1, 0, 2)[0].ToString());

            if (!registerContent.Contains("1"))
                registerContent[GenerateRandom(1, 0, registerContent.Count())[0]] = "1";
            polynomial.Add("1");


            return new KeyValuePair<List<String>, List<String>>(registerContent, polynomial);
        }
        #endregion

        #region Wygenerowanie nieparzystej liczby rejestrów
        private void MakeRegisters()
        {
            registers.Clear();
            for (int i = 0; i < howMuchRegisters; i++)
                registers.Add(CreateRegister());
            registersBackup = registers.Select(x => x).ToList();
        }
        #endregion

        #region Wygenerowanie klucza
        private String GenerateKey()
        {
            String key = "";
            int sum = 0;
            for (int i = 0; i < keyLength; i++)
            {
                sum = 0;
                for (int j = 0; j < registers.Count; j++) 
                {
                    var temp = registers[j];
                    sum += IterateOverRegister(ref temp);
                    registers[j] = temp;
                }
                if (sum > registers.Count / 2) key += "1";
                else key += "0";
                
            }
            return key;
        }
        #endregion

        #region Jedna iteracja po rejestrze
        private int IterateOverRegister(ref KeyValuePair<List<String>, List<String>> register)
        {
            int suma = 0;
            for(int i = 0; i < register.Key.Count; i++)
            {
                if (register.Key[i] == "1" && register.Value[i] == "1")
                    suma += 1;
            }
            int modulo = suma % 2;
            int pushedValue = int.Parse(register.Key.Last());
            List<String> temp = new List<String>();
            temp.Add(modulo.ToString());
            for (int i = 0; i < register.Key.Count - 1; i++)
                temp.Add(register.Key[i]);
            register = new KeyValuePair<List<String>, List<String>>(temp, register.Value);
            return pushedValue;
        }
        #endregion

    }
}
