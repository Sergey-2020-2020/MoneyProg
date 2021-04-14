using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace MoneyProgram
{
    class Program
    {
        //Фонд валют, для сравнения и простых операций, без сохранения результата.
        static private List<Money> currencyFund = new List<Money>();
        //Массив для пользовательских переменых. Пока думаю...
        static private List<Money> currencyBag = new List<Money>();

        static private string[] Currency = 
            {
                "Рубль", 
                "Доллар",
                "Евро"
            };
       static private void Main(string[] args)
        {
            //Функции инициализации
            //StartProgram();
            FakeStart();
            while (true)
            {
                var command = Console.ReadLine();
                // Выход из программы.
                if (command.ToLower() == "qqq")
                    return;
                //Функции проверки
                DoACommand(command);
            }
        }

        public class Money
        {
            public double Rate { get; private set; }
            public double Count { get; private set; }
            //todo: Сделать проверку на уникальность идентификатора!
            public string Identifier { get; private set; }
            public string MoneyName { get; private set; }
            public Money(double rate, string identifier, string name)
            {
                Rate = rate;
                Identifier = identifier;
                MoneyName = name;
                Count = 0;
            }
            public Money(Money money)
            {
                if (money == null)
                {
                    Console.WriteLine("Невозможно создать новый объект класса Money");
                }
                else {
                    Rate = money.Rate;
                    Identifier = money.Identifier;
                    MoneyName = money.MoneyName;
                    Count = money.Count;
                }

            }

            public string GetInfo()
            {
                return MoneyName + ": Курс = " + Rate + "; Идентификатор = " + Identifier + ".";
            }
            public void SetCount(string count)
            {
                if(new Regex(@"^(\d+(?:\,\d+)?){1}$").Match(count).Success)
                Count = Convert.ToDouble(count);
                else
                {
                    Console.WriteLine("Не удалось установить количество для валюты " + this.Identifier);
                }
            }
            public void SetRate(string rate)
            {
                if (new Regex(@"\d+\,\d+").Match(rate).Success)
                    Rate = Convert.ToDouble(rate);
                else
                {
                    Console.WriteLine("Введеное значение для изменения параметра Rate неправильное. (идентификатор: {0})", this.Identifier);
                }
            }
            public void SetIdentifier(string identifier)
            {
                if (identifier == "" || identifier == null)
                {
                    Console.WriteLine("Невозможно установить новое значение для идентификатора " + this.Identifier + " -> "+ identifier);
                }
                else { 
                    //todo Проверка на уникальность идентификатора
                Identifier = identifier;
                }
            }

            #region OperatorsOverload

            public static bool operator ==(Money money1, Money money2)
            {
                if (ReferenceEquals(money1, null) && ReferenceEquals(money2, null))
                    return true;
                if (ReferenceEquals(money1, null) || ReferenceEquals(money2, null))
                    return false;
                if (money1.Rate * money1.Count == money2.Rate * money2.Count)
                    return true;
                else
                    return false;
            }
            public static bool operator !=(Money money1, Money money2)
            {
                if (money1 == null || money2 == null)
                    return !(money1 == money2);
                if (money1.Rate * money1.Count != money2.Rate * money2.Count)
                    return true;
                else
                    return false;
            }
            public static bool operator >(Money money1, Money money2)
            {
                if (money1 == null || money2 == null)
                    return false;
                if (money1.Rate * money1.Count > money2.Rate * money2.Count)
                    return true;
                else
                    return false;
            }
            public static bool operator <(Money money1, Money money2)
            {
                if (money1 == null || money2 == null)
                    return false;
                if (money1.Rate * money1.Count < money2.Rate * money2.Count)
                    return true;
                else
                    return false;
            }
            public static bool operator >=(Money money1, Money money2)
            {
                if (money1 == null || money2 == null)
                    return false;
                if (money1.Rate * money1.Count >= money2.Rate * money2.Count)
                    return true;
                else
                    return false;
            }
            public static bool operator <=(Money money1, Money money2)
            {
                if (money1 == null || money2 == null)
                    return false;
                if (money1.Rate * money1.Count <= money2.Rate * money2.Count)
                    return true;
                else
                    return false;
            }
            public static Money operator +(Money money1, Money money2)
            {
                if (money1 == null || money2 == null)
                    return null;
                money1.Count = (money1.Rate * money1.Count + money2.Rate * money2.Count) / money1.Rate;
                return money1;
            }
            public static Money operator -(Money money1, Money money2)
            {
                if (money1 == null || money2 == null)
                    return null;
                money1.Count = (money1.Rate * money1.Count - money2.Rate * money2.Count) / money1.Rate;
                return money1;
            }

            #endregion


        }
        //Стартовое приветствие и инициализация всех валют.
        static private void StartProgram()
        {
            Console.WriteLine("Здравствуйте! Вас приветствует программа \"Финансовый Инверсионный Генератор\"");
            Console.WriteLine("Начнем с инициализации валют:");
            var count = Currency.Length;
            for (int i = 0; i < count; i++)
            {
                InitiateCurrenry(i);
            }


        }
        //Фейковая инициализация, для тестов.
        static public void FakeStart()
        {
            currencyFund.Add(new Money(1, "R", "Рубль"));
            currencyFund.Add(new Money(70, "D", "Доллар"));
            currencyFund.Add(new Money(50, "E", "Евро"));
        }
        //Инициализация всех валют
        static private void InitiateCurrenry(int curNum)
        {
            double rate;
            var curName = Currency[curNum];
            Console.WriteLine("Инициализация валюты \"" + curName + "\"");
            Console.WriteLine("Введите значение которое будет определять \"" + curName + "\"");
            var identifier = Console.ReadLine();
            
            if (curNum == 0)
            {
                Console.WriteLine("Данная валюта (" + curName + ") выбрана по умолчанию.");
                rate = 1;
            }
            else
            {
                do
                {
                    Console.WriteLine("Введите курс \"" + curName + "\". Пример: 30,00");
                    var rateString = Console.ReadLine();
                    if (new Regex(@"\d+\,\d+").Match(rateString).Success)
                    {
                        rate = Convert.ToDouble(rateString);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Значение не правильное!");
                    }
                } while (true);

            }
            currencyFund.Add(new Money(rate, identifier, curName));
        }
        //Функция для показа всех валют
        static public void ShowAll()
        {
            foreach (var money in currencyFund)
            {
                Console.WriteLine(money.GetInfo());
            }
        }
        //Функция для показа помощи...
        static public void ShowHelp()
        {
            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("Уникальные действия:");
            Console.WriteLine("help - вызвать текст помощи, если вы это читаете, значит вы его уже вызвали");
            Console.WriteLine("showall - показывает информацию обо всей валюте");
            Console.WriteLine("clear - очищает консоль.");
            Console.WriteLine("");
            Console.WriteLine("Начальные идентификаторы:");
            Console.WriteLine("? для операторов ставнения (==, !=, >, <, >=, <=)");
            Console.WriteLine("math для математических преобразований (+, -)");
            Console.WriteLine("");
            Console.WriteLine("Для ? и math запись ведется следующим образом:");
            Console.WriteLine("\"?/math 12.12D ==/+ 240R\", где первая часть - идентификатор вида действия, вторая и четвёртая части это деньги вида Число + Идентификатор " +
                              "(число может быть с точкой, но после точки обязательно должна идти цифра) и третья часть это оператор (указан в скобках для начальных идентификаторов выше)");
            Console.WriteLine("");
            Console.WriteLine("set для присвоения новых значения (Id, Rate)");
            Console.WriteLine("Примеры: \"set Rate R 10,12\" (запятая обязательна), \"set Id R Ruble\", первая часть - идентификатор вида действия, " +
                              "вторая - изменяемое поле, третья - идентификатор валюты, четвертая - новое значение.");

            Console.WriteLine("--------------------------------------------------------");
        }
        //Функция для распознования комманд
        static public void DoACommand(string command)
        {

            if (command.ToLower() == "showall")
            {
                ShowAll(); 
                return;
            }
            if (command.ToLower() == "help")
            {
                ShowHelp();
                return;
            }
            if (command.ToLower() == "goose")
            {
                Goose();
                return;
            }
            if (command.ToLower() == "nyan")
            {
                Nyan();
                return;
            }
            if (command.ToLower() == "clear")
            {
                Console.Clear();
                return;
            }


            var operators = command.Split(' ');
            switch (operators[0])
            {
                case "?": // ? 2D > 30R
                    if (operators.Length == 4)
                {
                    CompareMoney(operators[1], operators[3], operators[2]);
                    return;
                }
                    break;
                case "math": // math 10R + 2D
                    if (operators.Length == 4)
                    {
                        MathMoney(operators[1], operators[3], operators[2]);
                        return;
                    }
                    break;
                case "set": // set Rate R 10,12 (запятая обязательна)todo???
                    SetMoneySettings(operators[2], operators[1], operators[3]);
                    return;
                default:
                    break;

            }

            Console.WriteLine("Непонятная команда. Повторите ввод.");
            return;
        }
        //Функция сравнения
        static public void CompareMoney(string op1, string op2, string сomparator)
        {
            var m1 = RecognizeMoney(op1);
            var m2 = RecognizeMoney(op2);
            if (ValidateMoney(m1, m2))
                return;

            var money1 = new Money(m1);
            var money2 = new Money(m2);

            switch (сomparator)
            {
                case "==":
                    ComparisonResponse(money1 == money2);
                    break;
                case "!=":
                    ComparisonResponse(money1 != money2);
                    break;
                case ">":
                    ComparisonResponse(money1 > money2);
                    break;
                case "<":
                    ComparisonResponse(money1 < money2);
                    break;
                case ">=":
                    ComparisonResponse(money1 >= money2);
                    break;
                case "<=":
                    ComparisonResponse(money1 <= money2);
                    break;
                default:
                    Console.WriteLine("Не удалось определить оператор.");
                    break;
            }
        }
        //Функция распознования
        static public Money RecognizeMoney(string op) // Функция для определения денежного значения из строки
        {
            if (new Regex(@"^(\d+(?:\,\d+)?){1}(\w+){1}$").Match(op).Success)
            {
                var strArr = new Regex(@"^(\d+(?:\,\d+)?){1}(\w+){1}$").Split(op);
                var count = strArr[1];
                var identifier = strArr[2];
                var money = currencyFund.FirstOrDefault(m => m.Identifier == identifier);
                if(money == null)
                    return null;
                money.SetCount(count);
                return money;
            }

            return null;
        }
        //Функция ответа на сравнение
        static public void ComparisonResponse(bool response)
        {
            if(response)
                Console.WriteLine("Ответ: Да");
            else
                Console.WriteLine("Ответ: Нет");
        }

        static public void MathMoney(string op1, string op2, string сomparator)
        {
            var m1 = RecognizeMoney(op1);
            var m2 = RecognizeMoney(op2);
            if (ValidateMoney(m1, m2))
                return;

            var money1 = new Money(m1);
            var money2 = new Money(m2);

            switch (сomparator)
            {
                case "+":
                    MathResponse(money1 + money2);
                    break;
                case "-":
                    MathResponse(money1 - money2);
                    break;
                default:
                    Console.WriteLine("Не удалось определить оператор.");
                    break;
            }
        }

        static public void MathResponse(Money response)
        {
            Console.WriteLine("Ответ:" + response.Count + " " + response.MoneyName);
        }

        static public bool ValidateMoney(Money m1, Money m2)
        {
            // Вывод сообщений об ошибке 
            bool isError = false;
            if (m1 == null)
            {
                Console.WriteLine("Не удалось определить первое значение.");
                isError = true;
            }

            if (m2 == null)
            {
                Console.WriteLine("Не удалось определить второе значение.");
                isError = true;
            }

            if (isError)
                return true;
            return false;
        }

        static public void SetMoneySettings(string identifier, string setting, string newSet)
        {
            var money1 = currencyFund.Where(m => m.Identifier == identifier).FirstOrDefault();
            if (money1 == null) {
                Console.WriteLine("Не удалось определить тип валюты.");
                return;
            }
            switch (setting)
            {
                case "Id":
                    money1.SetIdentifier(newSet);
                    break;
                case "Rate":
                    money1.SetRate(newSet);
                    break;
                default:
                    Console.WriteLine("Не удалось определить тип настройки.");
                    break;
            }
        }

        #region . 

        static public void Goose()
        {
            Console.WriteLine("░░░░░░░░░░░░░░░░░░░░░▄▄▄░░░░");
            Console.WriteLine("░░░░░░░░░░░░░░░░░░░▄█████▄░░");
            Console.WriteLine("░░░░░░░░░░░░░░░░░░░████████▄");
            Console.WriteLine("░░░░░░░░░░░░░░░░░░░███░░░░░░");
            Console.WriteLine("░░░░░░░░░░░░░░░░░░░███░░░░░░");
            Console.WriteLine("░░░░░░░░░░░░░░░░░░░███░░░░░░");
            Console.WriteLine("░░░░░░░░░░░░░░░░░░░███░░░░░░");
            Console.WriteLine("░░░░░░░░░░░░░░░░░░░███░░░░░░");
            Console.WriteLine("░░░░░░░░░░░░░▄▄▄▄▄████░░░░░░");
            Console.WriteLine("░░░░░░░░▄▄████████████▄░░░░░");
            Console.WriteLine("░░░░▄▄██████████████████░░░░");
            Console.WriteLine("▄▄██████████████████████░░░░");
            Console.WriteLine("░▀▀████████████████████▀░░░░");
            Console.WriteLine("░░░░▀█████████████████▀░░░░░");
            Console.WriteLine("░░░░░░▀▀███████████▀▀░░░░░░░");
            Console.WriteLine("░░░░░░░░░▀███▀▀██▀░░░░░░░░░░");
            Console.WriteLine("░░░░░░░░░░█░░░░██░░░░░░░░░░░");
            Console.WriteLine("░░░░░░░░░░█░░░░█░░░░░░░░░░░░");
            Console.WriteLine("░░░▄▄▄▄███████▄███████▄▄▄▄░░");
        }
        static public void Nyan()
        {
            string nyan1  = "                                       ";
            string nyan3  = "          ▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄         ";
            string nyan4  = "        ▄▀░░░░░░░░░░░░▄░░░░░░░▀▄       ";
            string nyan5  = "        █░░▄░░░░▄░░░░░░░░░░░░░░█       ";
            string nyan6  = "        █░░░░░░░░░░░░▄█▄▄░░▄░░░█░▄▄▄   ";
            string nyan7  = " ▄▄▄▄▄  █░░░░░░▀░░░░▀█░░▀▄░░░░░█▀▀░██  ";
            string nyan8  = " ██▄▀██▄█░░░▄░░░░░░░██░░░░▀▀▀▀▀░░░░██  ";
            string nyan9  = "  ▀██▄▀██░░░░░░░░▀░██▀░░░░░░░░░░░░░▀██ ";
            string nyan10 = "    ▀████░▀░░░░▄░░░██░░░▄█░░░░▄░▄█░░██ ";
            string nyan11 = "       ▀█░░░░▄░░░░░██░░░░▄░░░▄░░▄░░░██ ";
            string nyan12 = "       ▄█▄░░░░░░░░░░░▀▄░░▀▀▀▀▀▀▀▀░░▄▀  ";
            string nyan13 = "      █▀▀█████████▀▀▀▀████████████▀    ";
            string nyan14 = "      ████▀  ███▀      ▀███  ▀██▀      ";
            string nyan15 = "                                       ";

            for (int i = 0; i < 80; i++)
            {
                Console.Clear();
                Console.WriteLine(nyan1 );
                Console.WriteLine(nyan3 );
                Console.WriteLine(nyan4 );
                Console.WriteLine(nyan5 );
                Console.WriteLine(nyan6 );
                Console.WriteLine(nyan7 );
                Console.WriteLine(nyan8 );
                Console.WriteLine(nyan9 );
                Console.WriteLine(nyan10);
                Console.WriteLine(nyan11);
                Console.WriteLine(nyan12);
                Console.WriteLine(nyan13);
                Console.WriteLine(nyan14);
                Console.WriteLine(nyan15);
                nyan1  = " " + nyan1 ;
                nyan3  = " " + nyan3 ;
                nyan4  = " " + nyan4 ;
                nyan5  = " " + nyan5 ;
                nyan6  = " " + nyan6 ;
                nyan7  = " " + nyan7 ;
                nyan8  = " " + nyan8 ;
                nyan9  = " " + nyan9 ;
                nyan10 = " " + nyan10;
                nyan11 = " " + nyan11;
                nyan12 = " " + nyan12;
                nyan13 = " " + nyan13;
                if(i%2==0)
                nyan14 = "  " + nyan14;
                nyan15 = " " + nyan15;
                Thread.Sleep(20);
            }
            Console.Clear();

        }
        #endregion

    }
}
