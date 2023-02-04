using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Explorer
{
    internal class Menu
    {
        private int _selectedIndex;
        private Dictionary<string, string> _options;
        private string _prompt = "Переключение между пунктами меню - стрелки вверх/вниз. Esc - возврат назад/выход. Enter - выбор";
        private Menu _parentMenu;
        private int min;
        private int max;

        public Menu(int min=0, int max=0)
        {
            // Public конструктор класса.
            _options = ExplorerWorker.GetDrives();
            _selectedIndex = 0;
            _parentMenu = null;
            this.min = min;
            this.max = max;
        }

        private Menu(string prompt, Dictionary<string, string> options, Menu parentMenu=null)
        {
            // Private конструктор класса, использующийся в рамках класса при открытии папок.
            _prompt += $"\n\n{prompt}\n";
            _options = options;
            _selectedIndex = 0;
            _parentMenu = parentMenu;
        }
        
        private void DisplayOptions()
        {
            // Метод для получения перечня опций.
            Console.WriteLine(_prompt);
            string[] options = _options.Values.ToArray();
            for (int i = 0; i < options.Length; i++)
            {
                string currentOption = options[i];
                string prefix;

                if (i == _selectedIndex)
                {
                    prefix = "->";
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.Gray;
                }
                else
                {
                    prefix = "";
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                Console.WriteLine($"{prefix}  {currentOption}");
                
            }
            Console.ResetColor();
        }

        public void Run()
        {
            // Метод, использующийся для запуска меню.
            ConsoleKey keyPressed;
            string[] options = _options.Values.ToArray();
            do
            {
                Clear();
                DisplayOptions();
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                keyPressed = keyInfo.Key;
            
                if (keyPressed == ConsoleKey.UpArrow)
                {
                    _selectedIndex--;
                    if (_selectedIndex == -1)
                        _selectedIndex = options.Length - 1;
                }
                else if (keyPressed == ConsoleKey.DownArrow)
                {
                    _selectedIndex++;
                    if (_selectedIndex == options.Length)
                        _selectedIndex = 0;
                }
                if (keyPressed == ConsoleKey.Enter)
                {
                    string selectedPath = _options.ElementAt(_selectedIndex).Key;
                    if (!ExplorerWorker.OpenFile(selectedPath))
                    {
                        string childPrompt = $"{selectedPath}";
                        var childOptions = ExplorerWorker.GetFilesAndDirs(selectedPath);
                        if(childOptions.Count != 0)
                        {
                            Menu child = new Menu(childPrompt, childOptions, this);
                            Clear();
                            child.Run();
                            break;
                        }
                        MessageBox.Show("Данная папка пуста, переход в нее невозможен.", "Ошибка");
                        
                    }
                }
                if (keyPressed == ConsoleKey.Escape)
                {
                    if(_parentMenu is null)
                        Environment.Exit(0);
                    
                    Clear();
                    _parentMenu.Run();
                    break;
                }

            } while (true);
        }

        private void Clear()
        {
            // Метод, использующийся для очистки консоли (заполнения пробелами каждой строки).
            for(int i = 1; i <= _options.Count + 4; i++)
                Console.MoveBufferArea(0, i, Console.BufferWidth, 1, Console.BufferWidth, i, ' ', Console.ForegroundColor, Console.BackgroundColor);
            
            Console.SetCursorPosition(0, 0);
        }

    }
}

