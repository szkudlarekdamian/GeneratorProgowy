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
using System.Threading;
using System.Runtime.CompilerServices;
using System.IO;
using System.ComponentModel;

namespace GeneratorProgowy
{

    public partial class MainWindow : Window
    {
        private int howMuchRegisters = 3,
            maximumLengthOfRegister = 20,
            keyLength = 20000;
        private bool stop = false;
        private String keyGlobal = "";

        private static Random random = new Random();
        private List<int> registersLengths = new List<int>();
        private List<KeyValuePair<List<String>, List<String>>> registers = new List<KeyValuePair<List<String>, List<String>>>();
        private List<KeyValuePair<List<String>, List<String>>> registersBackup = new List<KeyValuePair<List<String>, List<String>>>();
        private List<KeyValuePair<short, List<String>>> perfectPolynomians = new List<KeyValuePair<short, List<String>>>();


        public MainWindow()
        {
            CreatePerfectPolynomians();
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

        #region Okienka informacyjne
        private void showAlert(String text)
        {
            string caption = "Uwaga";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Information;
            MessageBox.Show(text, caption, button, icon);
        }
        #endregion

        #region Stałe wielomiany
        private void CreatePerfectPolynomians()
        {
            perfectPolynomians.Add(new KeyValuePair<short, List<String>>(3, CreatePolynomian(new List<short>() { 4, 1, 0 })));
            perfectPolynomians.Add(new KeyValuePair<short, List<String>>(4, CreatePolynomian(new List<short>() { 5, 2, 0 })));
            perfectPolynomians.Add(new KeyValuePair<short, List<String>>(5, CreatePolynomian(new List<short>() { 6, 1, 0 }) ));
            perfectPolynomians.Add(new KeyValuePair<short, List<String>>(6, CreatePolynomian(new List<short>() { 7, 1, 0 })));
            perfectPolynomians.Add(new KeyValuePair<short, List<String>>(7, CreatePolynomian(new List<short>() { 8, 4, 3, 2, 0 })));
            perfectPolynomians.Add(new KeyValuePair<short, List<String>>(8, CreatePolynomian(new List<short>() { 9, 4, 0 })));
            perfectPolynomians.Add(new KeyValuePair<short, List<String>>(9, CreatePolynomian(new List<short>() { 10, 3, 0 })));
            perfectPolynomians.Add(new KeyValuePair<short, List<String>>(10, CreatePolynomian(new List<short>() { 11, 2, 0 })));
            perfectPolynomians.Add(new KeyValuePair<short, List<String>>(11, CreatePolynomian(new List<short>() { 12, 6, 4, 1, 0 })));
            perfectPolynomians.Add(new KeyValuePair<short, List<String>>(12, CreatePolynomian(new List<short>() { 13, 4, 3, 1, 0 })));
            perfectPolynomians.Add(new KeyValuePair<short, List<String>>(13, CreatePolynomian(new List<short>() { 14, 5, 3, 1, 0 })));
            perfectPolynomians.Add(new KeyValuePair<short, List<String>>(14, CreatePolynomian(new List<short>() { 15, 1, 0 })));
            perfectPolynomians.Add(new KeyValuePair<short, List<String>>(15, CreatePolynomian(new List<short>() { 16, 5, 3, 2, 0 })));
            perfectPolynomians.Add(new KeyValuePair<short, List<String>>(16, CreatePolynomian(new List<short>() { 17, 3, 0 })));
            perfectPolynomians.Add(new KeyValuePair<short, List<String>>(17, CreatePolynomian(new List<short>() { 18, 5, 2, 1, 0 })));
            perfectPolynomians.Add(new KeyValuePair<short, List<String>>(18, CreatePolynomian(new List<short>() { 19, 5, 2, 1, 0 })));
            perfectPolynomians.Add(new KeyValuePair<short, List<String>>(19, CreatePolynomian(new List<short>() { 20, 3, 0 })));
            perfectPolynomians.Add(new KeyValuePair<short, List<String>>(20, CreatePolynomian(new List<short>() { 21, 2, 0 })));
            perfectPolynomians.Add(new KeyValuePair<short, List<String>>(21, CreatePolynomian(new List<short>() { 22, 1, 0 })));
            perfectPolynomians.Add(new KeyValuePair<short, List<String>>(22, CreatePolynomian(new List<short>() { 23, 5, 0 })));
            perfectPolynomians.Add(new KeyValuePair<short, List<String>>(23, CreatePolynomian(new List<short>() { 24, 4, 3, 1, 0 })));
            perfectPolynomians.Add(new KeyValuePair<short, List<String>>(24, CreatePolynomian(new List<short>() { 25, 3, 0 })));
            perfectPolynomians.Add(new KeyValuePair<short, List<String>>(25, CreatePolynomian(new List<short>() { 26, 6, 2, 1, 0 })));
        }

        private List<String> CreatePolynomian(List<short> list)
        {
            short length = (short)(list.ElementAt(0) - 1);
            String returnString = "";
            for(short i = 0; i<(length-list.Count()+1); i++)
            {
                returnString += "0";
            }
            List<short> dummy = list;
            dummy.RemoveAt(0);
            foreach(short index in dummy)
            {
                short position = (short)(length - (index + 1));
                returnString = returnString.Insert(position, "1");
            }
            return returnString.Select(x=> x.ToString()).ToList();

        }
        #endregion

        #region Utworzenie rejestru
        private KeyValuePair<List<String>, List<String>> CreateRegister()
        {
            List<String> registerContent = new List<String>();
            List<String> polynomial = new List<String>();
            int randomNumber = -1;
            do
            {
                randomNumber = GenerateRandom(1, 3, maximumLengthOfRegister + 1)[0];
            } while (registersLengths.Contains(randomNumber));
            registersLengths.Add(randomNumber);

            for (int i = 0; i < randomNumber - 1; i++)
            {
                polynomial.Add(GenerateRandom(1, 0, 2)[0].ToString());
            }
            for (int i = 0; i < randomNumber; i++)
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
            registersLengths.Clear();
            for (int i = 0; i < howMuchRegisters; i++)
                registers.Add(CreateRegister());
            //registersBackup = registers.Select(y => y).ToList();
            registersBackup = CopyList(registers);
            
        }
        #endregion

        #region Głęboka kopia listy

        private List<KeyValuePair<List<string>, List<string>>> CopyList(List<KeyValuePair<List<string>, List<string>>> list)
        {
            return list.ConvertAll(kvp => new KeyValuePair<List<string>, List<string>>(kvp.Key, kvp.Value));
        }

        #endregion

        #region Wygenerowanie klucza
        private String GenerateKey()
        {
            String key = "";
            registers = CopyList(registersBackup);
            int sum = 0;
            int count = registers.Count;
            int countDivided = count / 2;
            for (int i = 0; i < keyLength; i++)
            {
                sum = 0;
                for (int j = 0; j < count; j++)
                {
                    var temp = registers[j];
                    sum += IterateOverRegister(ref temp);
                    registers[j] = temp;
                }
                if (sum > countDivided) key += "1";
                else key += "0";

                if (stop == true)
                {
                    break;
                }

            }
            return key;
        }

        #endregion

        #region Jedna iteracja po rejestrze
        private int IterateOverRegister(ref KeyValuePair<List<String>, List<String>> register)
        {
            int sum = 0;
            for (int i = 0; i < register.Key.Count; i++)
            {
                if (register.Key[i] == "1" && register.Value[i] == "1")
                    sum += 1;
            }
            int pushedValue = int.Parse(register.Key.Last());
            //List<String> temp = new List<String>();
            //temp.Add((sum % 2).ToString());
            //temp = register.Key.GetRange(0, register.Key.Count-1);
            register.Key.RemoveAt(register.Key.Count-1);
            register.Key.Insert(0, (sum % 2).ToString());
            //register = new KeyValuePair<List<String>, List<String>>(temp, register.Value);
            return pushedValue;
        }

        #endregion

        #region EventyXAMLA
        private void howMuchRegistersComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (howMuchRegistersComboBox.Items.Count > 1)
            {
                int howM = int.Parse(howMuchRegistersComboBox.SelectedItem.ToString().Last().ToString());
                slValue.Minimum = howM + 2;
            }
        }

        private void maximumLengthOfRegisterTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            System.Text.RegularExpressions.Regex _regex = new System.Text.RegularExpressions.Regex("[^0-9]+$");
            e.Handled = _regex.IsMatch(e.Text);
        }

        private void keyLengthTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            System.Text.RegularExpressions.Regex _regex = new System.Text.RegularExpressions.Regex("[^0-9]+$");
            e.Handled = _regex.IsMatch(e.Text);
        }

        private void keyLengthTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            keyLengthTextBox.Text = string.Concat(keyLengthTextBox.Text.Where(x => char.IsDigit(x)).Select(x => x));
        }

        private void maximumLengthOfRegisterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            maximumLengthOfRegisterTextBox.Text = string.Concat(maximumLengthOfRegisterTextBox.Text.Where(x => char.IsDigit(x)).Select(x => x));
        }

        private void generateRegisters_Click(object sender, RoutedEventArgs e)
        {
            System.Text.RegularExpressions.Regex _regex = new System.Text.RegularExpressions.Regex("[^0-9]+$");
            if (!_regex.IsMatch(maximumLengthOfRegisterTextBox.Text))
            {
                howMuchRegisters = int.Parse(howMuchRegistersComboBox.SelectedItem.ToString().Last().ToString());
                maximumLengthOfRegister = int.Parse(maximumLengthOfRegisterTextBox.Text);
                MakeRegisters();

                if (perfectPolynomiansRadioButton.IsChecked == true)
                {
                    for (short p = 0; p < registers.Count(); p++)
                    {
                        var p1 = registers[p].Key;
                        var p2 = perfectPolynomians.Where(z => z.Key == p1.Count()).Select(x => x.Value).ToList();
                        registers.RemoveAt(p);
                        registers.Insert(p, new KeyValuePair<List<string>, List<string>>(p1, p2[0]));

                    }
                }
                registersBackup = CopyList(registers);

                fillTheRegistersStackPanel();

                keyLengthWrapPanel.Visibility = Visibility.Visible;
                generateKeyBtn.Visibility = Visibility.Visible;
            }
            else
            {
                showAlert("Nieprawidłowo uzupełniono pole: 'Maksymalna dlugość rejestru'");
            }
        }

        private void fillTheRegistersStackPanel()
        {
            generatedRegistersStackPanel.Children.Clear();

            int i = 1, j = 1;

            foreach (var register in registers)
            {
                StackPanel stack = new StackPanel { Name = "stackPanel" + i.ToString(), Margin = new Thickness(0, 2, 0, 2) };
                WrapPanel wrapX = new WrapPanel { Name = "wrapPanelX" + i.ToString(), Margin = new Thickness(0, 2, 0, 0) };
                WrapPanel wrapA = new WrapPanel { Name = "wrapPanelA" + i.ToString(), Margin = new Thickness(0, 2, 0, 0) };
                stack.Children.Add(new Label { Content = "LFSR " + i.ToString(), Margin = new Thickness(0, 0, 0, 0), FontWeight = FontWeights.DemiBold });
                wrapX.Children.Add(new Label { Content = "x:", Margin = new Thickness(0, 0, 14, 0) });
                wrapA.Children.Add(new Label { Content = "a:", Margin = new Thickness(0, 0, 0, 0) });
                wrapA.Children.Add(new TextBox { Name = "textboxA" + i.ToString() + "Model", Margin = new Thickness(1, 0, 0, 0), FontSize = 9, Text = "1", Width = 13, Height = 13, HorizontalContentAlignment = HorizontalAlignment.Center, VerticalContentAlignment = VerticalAlignment.Center, IsReadOnly = true });

                foreach (var xi in register.Key)
                {
                    wrapX.Children.Add(new TextBox { Name = "textboxX" + i.ToString() + j.ToString(), Margin = new Thickness(0.4, 0, 0, 0), FontSize = 9, Text = xi.ToString(), Width = 13, Height = 13, HorizontalContentAlignment = HorizontalAlignment.Center, VerticalContentAlignment = VerticalAlignment.Center, IsReadOnly = true });
                    j++;
                }
                j = 1;
                foreach (var ai in register.Value)
                {
                    wrapA.Children.Add(new TextBox { Name = "textboxA" + i.ToString() + j.ToString(), Margin = new Thickness(0.4, 0, 0, 0), FontSize = 9, Text = ai.ToString(), Width = 13, Height = 13, HorizontalContentAlignment = HorizontalAlignment.Center, VerticalContentAlignment = VerticalAlignment.Center, IsReadOnly = true });
                    j++;
                }
                j = 1;
                i++;
                stack.Children.Add(wrapX);
                stack.Children.Add(wrapA);
                stack.Children.Add(new Border { BorderBrush = Brushes.Black, BorderThickness = new Thickness(0, 1, 0, 0), Margin = new Thickness(15, 2, 15, 2) });
                generatedRegistersStackPanel.Children.Add(stack);
            }
        }

        private void stopKeyBtn_Click(object sender, RoutedEventArgs e)
        {
            stop = true;
        }

        private void keySaveButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog
            {
                DefaultExt = ".txt",
                Filter = "Pliki tekstowe (.txt)|*.txt"
            };

            Nullable<bool> result = dlg.ShowDialog();
            if (result == true && keyGlobal != "")
                File.WriteAllText(dlg.FileName, keyGlobal, Encoding.GetEncoding("Windows-1250"));
            else
                showAlert("Błąd przy zapisie do pliku.");
        }

        private void infoSaveButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog
            {
                DefaultExt = ".txt",
                Filter = "Pliki tekstowe (.txt)|*.txt"
            };
            String returnValue = "";
            foreach (var register in registers)
            {
                returnValue += String.Concat(String.Concat(register.Key), ";", String.Concat(register.Value), Environment.NewLine);
            }

            Nullable<bool> result = dlg.ShowDialog();
            if (result == true && returnValue != "")
                File.WriteAllText(dlg.FileName, returnValue, Encoding.GetEncoding("Windows-1250"));
            else
                showAlert("Błąd przy zapisie do pliku.");
        }

        private void loadRegisters_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".txt",
                Filter = "Pliki tekstowe (.txt)|*.txt"
            };
            Nullable<bool> result = dlg.ShowDialog();
            if(result == true)
            {
                String[] xa;
                String x, a;
                List<String> aL = new List<String>(), xL = new List<String>();
                List<KeyValuePair<List<String>, List<String>>> registersTemp = new List<KeyValuePair<List<string>, List<string>>>();
                registers.Clear();
                registersBackup.Clear();
                var lines = File.ReadLines(dlg.FileName);

                foreach(var line in lines)
                {
                    xa = line.Split(';');
                    x = xa[0];
                    a = xa[1];
                    if (!a.Last().Equals('1')) { a.Remove(a.Length - 1, 0); a += '1'; }
                    if (!x.Contains('0')) { x.Remove(0, 1); x += '1'; }
                    if (a == "") aL = perfectPolynomians.Where(z => z.Key == x.Length).Select(y => y.Value).ToList()[0];
                    else aL = a.Select(y => y.ToString()).ToList();
                    xL = x.Select(y => y.ToString()).ToList();
                    registers.Add(new KeyValuePair<List<string>, List<string>>(xL, aL));
                }
                registersBackup = CopyList(registers);
                fillTheRegistersStackPanel();
                keyLengthWrapPanel.Visibility = Visibility.Visible;
                generateKeyBtn.Visibility = Visibility.Visible;
            }
        }

        private void generateKeyBtn_Click(object sender, RoutedEventArgs e)
        {
            if (keyLengthTextBox.Text != "")
            {
                keyTextBox.Text = "";
                counter.Text = "0";
                time.Text = "0m 0s 0ms";
                keyLength = int.Parse(keyLengthTextBox.Text);
                stopKeyBtn.Visibility = Visibility.Visible;
                ThreadPool.QueueUserWorkItem(ThreadProc, keyLengthTextBox);
                generateKeyBtn.IsEnabled = false;
                generateRegisters.IsEnabled = false;
                loadRegisters.IsEnabled = false;

                keySaveButton.Visibility = Visibility.Hidden;
                infoSaveButton.Visibility = Visibility.Hidden;
            }
            else
                showAlert("Nie podano długości klucza!");
        }

        private void ThreadProc(object state)
        {
            DateTime startTime = DateTime.Now;
            keyGlobal = GenerateKey();
            DateTime endTime = DateTime.Now;
            TimeSpan span = endTime.Subtract(startTime);
            keyTextBox.Dispatcher.Invoke(
                 System.Windows.Threading.DispatcherPriority.Normal,
                (ThreadStart)delegate
                {
                    keyTextBox.Text = keyGlobal;
                    counter.Text = keyGlobal.Count().ToString();
                    time.Text = span.Minutes + "m " + span.Seconds + "s " + span.Milliseconds + "ms";
                    generateKeyBtn.IsEnabled = true;
                    generateRegisters.IsEnabled = true;
                    loadRegisters.IsEnabled = true;
                    keySaveButton.Visibility = Visibility.Visible;
                    infoSaveButton.Visibility = Visibility.Visible;
                    stopKeyBtn.Visibility = Visibility.Hidden;
                }
            );
            stop = false;
        }

        #endregion

    }
}
