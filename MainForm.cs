using System;
using System.Drawing;
using System.Windows.Forms;

namespace SmartTaskDistributor
{
    public class MainForm : Form
    {
        private readonly TaskManager _taskManager = new TaskManager();

        private readonly TextBox _txtTaskName = new TextBox();
        private readonly NumericUpDown _numDuration = new NumericUpDown();
        private readonly NumericUpDown _numDifficulty = new NumericUpDown();
        private readonly ComboBox _cmbPriority = new ComboBox();
        private readonly ListBox _lstTasks = new ListBox();
        private readonly ListBox _lstSchedule = new ListBox();
        private readonly DateTimePicker _timeStart = new DateTimePicker();
        private readonly Label _lblSummary = new Label();

        public MainForm()
        {
            InitializeUi();
        }

        private void InitializeUi()
        {
            Text = "Smart Task Distributor";
            Size = new Size(1100, 700);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = ColorTranslator.FromHtml("#1E1E2F");
            Font = new Font("Segoe UI", 10f);

            Label title = new Label();
            title.Text = "Smart Task Distributor";
            title.ForeColor = Color.White;
            title.Font = new Font("Segoe UI Semibold", 20f);
            title.Location = new Point(25, 20);
            title.AutoSize = true;

            Label taskHint = new Label();
            taskHint.Text = "Название задачи";
            taskHint.ForeColor = Color.Gainsboro;
            taskHint.Location = new Point(25, 68);
            taskHint.AutoSize = true;

            _txtTaskName.SetBounds(25, 90, 280, 36);

            _numDuration.SetBounds(320, 90, 130, 36);
            _numDuration.Minimum = 1;
            _numDuration.Maximum = 480;
            _numDuration.Value = 30;

            _numDifficulty.SetBounds(465, 90, 130, 36);
            _numDifficulty.Minimum = 1;
            _numDifficulty.Maximum = 5;
            _numDifficulty.Value = 3;

            _cmbPriority.SetBounds(610, 90, 140, 36);
            _cmbPriority.DropDownStyle = ComboBoxStyle.DropDownList;
            _cmbPriority.Items.AddRange(new object[] { "Низкий", "Средний", "Высокий" });
            _cmbPriority.SelectedIndex = 1;

            RoundedButton btnAdd = CreateButton("Добавить", 765, 90, OnAddClick, Color.FromArgb(76, 175, 80));
            RoundedButton btnRemove = CreateButton("Удалить", 25, 145, OnRemoveClick, Color.FromArgb(0, 194, 255));
            RoundedButton btnOptimize = CreateButton("Оптимизировать", 170, 145, OnOptimizeClick, Color.FromArgb(76, 175, 80));
            RoundedButton btnPlan = CreateButton("Составить план", 315, 145, OnBuildPlanClick, Color.FromArgb(76, 175, 80));
            RoundedButton btnClear = CreateButton("Очистить", 490, 145, OnClearClick, Color.FromArgb(225, 87, 89));

            Label startLabel = new Label();
            startLabel.Text = "Начало дня:";
            startLabel.ForeColor = Color.Gainsboro;
            startLabel.Location = new Point(670, 152);
            startLabel.AutoSize = true;

            _timeStart.Format = DateTimePickerFormat.Custom;
            _timeStart.CustomFormat = "HH:mm";
            _timeStart.ShowUpDown = true;
            _timeStart.Value = DateTime.Today.AddHours(9);
            _timeStart.SetBounds(760, 145, 100, 36);

            Label tasksLabel = new Label();
            tasksLabel.Text = "Список задач";
            tasksLabel.ForeColor = Color.White;
            tasksLabel.Location = new Point(25, 190);
            tasksLabel.AutoSize = true;

            _lstTasks.SetBounds(25, 215, 500, 400);
            _lstTasks.BackColor = Color.FromArgb(35, 35, 50);
            _lstTasks.ForeColor = Color.White;
            _lstTasks.BorderStyle = BorderStyle.None;

            Label scheduleLabel = new Label();
            scheduleLabel.Text = "Расписание";
            scheduleLabel.ForeColor = Color.White;
            scheduleLabel.Location = new Point(545, 190);
            scheduleLabel.AutoSize = true;

            _lstSchedule.SetBounds(545, 215, 520, 400);
            _lstSchedule.BackColor = Color.FromArgb(35, 35, 50);
            _lstSchedule.ForeColor = Color.White;
            _lstSchedule.BorderStyle = BorderStyle.None;

            _lblSummary.SetBounds(25, 630, 1040, 30);
            _lblSummary.ForeColor = Color.FromArgb(200, 200, 220);
            _lblSummary.Text = "Задач: 0 | Общее время: 0 мин | Загруженность: Низкая";

            Controls.Add(title);
            Controls.Add(taskHint);
            Controls.Add(_txtTaskName);
            Controls.Add(_numDuration);
            Controls.Add(_numDifficulty);
            Controls.Add(_cmbPriority);
            Controls.Add(btnAdd);
            Controls.Add(btnRemove);
            Controls.Add(btnOptimize);
            Controls.Add(btnPlan);
            Controls.Add(btnClear);
            Controls.Add(startLabel);
            Controls.Add(_timeStart);
            Controls.Add(tasksLabel);
            Controls.Add(_lstTasks);
            Controls.Add(scheduleLabel);
            Controls.Add(_lstSchedule);
            Controls.Add(_lblSummary);
        }

        private RoundedButton CreateButton(string text, int x, int y, EventHandler onClick, Color color)
        {
            RoundedButton button = new RoundedButton();
            button.Text = text;
            button.NormalBackColor = color;
            button.HoverBackColor = ControlPaint.Light(color, .2f);
            button.BackColor = color;
            button.SetBounds(x, y, 160, 40);
            button.Click += onClick;
            return button;
        }

        private void OnAddClick(object sender, EventArgs e)
        {
            try
            {
                string name = _txtTaskName.Text.Trim();
                if (string.IsNullOrWhiteSpace(name) || _cmbPriority.SelectedItem == null)
                {
                    MessageBox.Show("Введите название задачи и выберите приоритет.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int duration = Convert.ToInt32(_numDuration.Value);
                int difficulty = Convert.ToInt32(_numDifficulty.Value);
                string priority = _cmbPriority.SelectedItem.ToString();

                if (duration <= 0 || (difficulty < 1 || difficulty > 5))
                {
                    MessageBox.Show("Проверьте длительность и сложность задачи.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _taskManager.AddTask(new TaskItem(name, duration, difficulty, priority));
                _txtTaskName.Clear();
                RefreshTaskList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось добавить задачу: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnRemoveClick(object sender, EventArgs e)
        {
            TaskItem selectedTask = _lstTasks.SelectedItem as TaskItem;
            if (selectedTask == null)
            {
                MessageBox.Show("Выберите задачу для удаления.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _taskManager.RemoveTask(selectedTask);
            RefreshTaskList();
        }

        private void OnOptimizeClick(object sender, EventArgs e)
        {
            _taskManager.SortTasks();
            RefreshTaskList();
        }

        private void OnBuildPlanClick(object sender, EventArgs e)
        {
            if (_taskManager.Tasks.Count == 0)
            {
                MessageBox.Show("Добавьте хотя бы одну задачу для составления плана.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _taskManager.SortTasks();
            BuildSchedule();
            RefreshTaskList();
        }

        private void BuildSchedule()
        {
            _lstSchedule.Items.Clear();
            DateTime current = DateTime.Today.AddHours(_timeStart.Value.Hour).AddMinutes(_timeStart.Value.Minute);

            foreach (TaskItem task in _taskManager.Tasks)
            {
                DateTime start = current;
                DateTime end = start.AddMinutes(task.Duration);
                string line = start.ToString("HH:mm") + " - " + end.ToString("HH:mm") + " | " + task.Name;
                _lstSchedule.Items.Add(line);
                current = end;
            }

            _lstSchedule.Items.Add("---------------------------");
            _lstSchedule.Items.Add("План завершится в: " + current.ToString("HH:mm"));
        }

        private void OnClearClick(object sender, EventArgs e)
        {
            _taskManager.ClearTasks();
            _lstSchedule.Items.Clear();
            RefreshTaskList();
        }

        private void RefreshTaskList()
        {
            _lstTasks.Items.Clear();
            foreach (TaskItem task in _taskManager.Tasks)
            {
                _lstTasks.Items.Add(task);
            }

            int total = _taskManager.GetTotalTime();
            string loadLevel;
            if (total <= 120)
            {
                loadLevel = "Низкая";
            }
            else if (total <= 300)
            {
                loadLevel = "Средняя";
            }
            else
            {
                loadLevel = "Высокая";
            }

            _lblSummary.Text = "Задач: " + _taskManager.Tasks.Count + " | Общее время: " + total.ToString() + " мин | Загруженность: " + loadLevel;
        }
    }
}
