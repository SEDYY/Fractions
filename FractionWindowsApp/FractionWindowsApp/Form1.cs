using System;
using System.Drawing;
using System.Windows.Forms;

public class Fraction
{
    private long integerPart;
    private ushort fractionalPart;

    public Fraction(long integer, ushort fractional)
    {
        if (fractional > 999)
            throw new ArgumentException("������� ����� ������ ���� �� 0 �� 999.");
        integerPart = integer;
        fractionalPart = fractional;
    }

    public long IntegerPart
    {
        get { return integerPart; }
        set { integerPart = value; }
    }

    public ushort FractionalPart
    {
        get { return fractionalPart; }
        set
        {
            if (value > 999)
                throw new ArgumentException("������� ����� ������ ���� �� 0 �� 999.");
            fractionalPart = value;
        }
    }

    private decimal ToDecimal()
    {
        return integerPart + (fractionalPart / 1000m);
    }

    public static Fraction operator +(Fraction a, Fraction b)
    {
        decimal sum = a.ToDecimal() + b.ToDecimal();
        if (sum > long.MaxValue || sum < long.MinValue)
            throw new OverflowException("��������� �������� ��������� ���������� ��������.");
        long integer = (long)sum;
        ushort fractional = (ushort)Math.Round(Math.Abs(sum - integer) * 1000, 0);
        if (fractional > 999) fractional = 999;
        return new Fraction(integer, fractional);
    }

    public static Fraction operator -(Fraction a, Fraction b)
    {
        decimal diff = a.ToDecimal() - b.ToDecimal();
        if (diff > long.MaxValue || diff < long.MinValue)
            throw new OverflowException("��������� ��������� ��������� ���������� ��������.");
        long integer = (long)diff;
        ushort fractional = (ushort)Math.Round(Math.Abs(diff - integer) * 1000, 0);
        if (fractional > 999) fractional = 999;
        return new Fraction(integer, fractional);
    }

    public static Fraction operator *(Fraction a, Fraction b)
    {
        decimal product = a.ToDecimal() * b.ToDecimal();
        if (product > long.MaxValue || product < long.MinValue)
            throw new OverflowException("��������� ��������� ��������� ���������� ��������.");
        long integer = (long)product;
        ushort fractional = (ushort)Math.Round(Math.Abs(product - integer) * 1000, 0);
        if (fractional > 999) fractional = 999;
        return new Fraction(integer, fractional);
    }

    public static bool operator ==(Fraction a, Fraction b)
    {
        if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            return false;
        return Math.Abs(a.ToDecimal() - b.ToDecimal()) < 0.0001m;
    }

    public static bool operator !=(Fraction a, Fraction b)
    {
        return !(a == b);
    }

    public static bool operator <(Fraction a, Fraction b)
    {
        return a.ToDecimal() < b.ToDecimal();
    }

    public static bool operator >(Fraction a, Fraction b)
    {
        return a.ToDecimal() > b.ToDecimal();
    }

    public static bool operator <=(Fraction a, Fraction b)
    {
        return a.ToDecimal() <= b.ToDecimal();
    }

    public static bool operator >=(Fraction a, Fraction b)
    {
        return a.ToDecimal() >= b.ToDecimal();
    }

    public override string ToString()
    {
        return $"{integerPart}.{fractionalPart:D3}";
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;
        return this == (Fraction)obj;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + integerPart.GetHashCode();
            hash = hash * 23 + fractionalPart.GetHashCode();
            return hash;
        }
    }
}

public class FractionCalculatorForm : Form
{
    private readonly TextBox txtInt1 = new TextBox();
    private readonly TextBox txtFrac1 = new TextBox();
    private readonly TextBox txtInt2 = new TextBox();
    private readonly TextBox txtFrac2 = new TextBox();
    private readonly Button btnAdd = new Button();
    private readonly Button btnSubtract = new Button();
    private readonly Button btnMultiply = new Button();
    private readonly Button btnCompare = new Button();
    private readonly Label lblResult = new Label();
    private readonly Label lblResultTitle = new Label();
    private readonly Panel panelFirstFraction = new Panel();
    private readonly Panel panelSecondFraction = new Panel();
    private readonly Panel panelButtons = new Panel();
    private readonly Panel panelResult = new Panel();
    private readonly ToolTip toolTip = new ToolTip();

    public FractionCalculatorForm()
    {
        try
        {
            InitializeComponents();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"������ ��� ������������� �����: {ex.Message}", "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
            throw;
        }
    }

    private void InitializeComponents()
    {
        // ��������� �����
        this.Text = "����������� ������";
        this.Size = new Size(480, 380);
        this.BackColor = Color.FromArgb(30, 30, 30); // Ҹ���� ���
        this.Font = new Font(SystemFonts.DefaultFont.FontFamily.Name, 10); // �������� �����
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;

        // ������ ��� ������ �����
        panelFirstFraction.Location = new Point(20, 20);
        panelFirstFraction.Size = new Size(210, 110);
        panelFirstFraction.BorderStyle = BorderStyle.FixedSingle;
        panelFirstFraction.BackColor = Color.FromArgb(45, 45, 45);
        panelFirstFraction.Parent = this;

        // ����� � ���� ��� ������ �����
        var lblFirstFraction = new Label
        {
            Text = "������ �����",
            Location = new Point(10, 10),
            Size = new Size(190, 20),
            Font = new Font(SystemFonts.DefaultFont.FontFamily.Name, 11, FontStyle.Bold),
            ForeColor = Color.White
        };
        lblFirstFraction.Parent = panelFirstFraction;

        var lblInt1 = new Label
        {
            Text = "����� �����:",
            Location = new Point(10, 40),
            Size = new Size(80, 20),
            ForeColor = Color.White
        };
        lblInt1.Parent = panelFirstFraction;

        txtInt1.Location = new Point(100, 38);
        txtInt1.Size = new Size(90, 24);
        txtInt1.BackColor = Color.FromArgb(60, 60, 60);
        txtInt1.ForeColor = Color.White;
        txtInt1.BorderStyle = BorderStyle.FixedSingle;
        txtInt1.Parent = panelFirstFraction;
        toolTip.SetToolTip(txtInt1, "������� ����� ����� ����� (��������, 2)");

        var lblFrac1 = new Label
        {
            Text = "������� �����:",
            Location = new Point(10, 70),
            Size = new Size(80, 20),
            ForeColor = Color.White
        };
        lblFrac1.Parent = panelFirstFraction;

        txtFrac1.Location = new Point(100, 68);
        txtFrac1.Size = new Size(90, 24);
        txtFrac1.BackColor = Color.FromArgb(60, 60, 60);
        txtFrac1.ForeColor = Color.White;
        txtFrac1.BorderStyle = BorderStyle.FixedSingle;
        txtFrac1.Parent = panelFirstFraction;
        toolTip.SetToolTip(txtFrac1, "������� ������� ����� (0�999, ��������, 500)");

        // ������ ��� ������ �����
        panelSecondFraction.Location = new Point(240, 20);
        panelSecondFraction.Size = new Size(210, 110);
        panelSecondFraction.BorderStyle = BorderStyle.FixedSingle;
        panelSecondFraction.BackColor = Color.FromArgb(45, 45, 45);
        panelSecondFraction.Parent = this;

        // ����� � ���� ��� ������ �����
        var lblSecondFraction = new Label
        {
            Text = "������ �����",
            Location = new Point(10, 10),
            Size = new Size(190, 20),
            Font = new Font(SystemFonts.DefaultFont.FontFamily.Name, 11, FontStyle.Bold),
            ForeColor = Color.White
        };
        lblSecondFraction.Parent = panelSecondFraction;

        var lblInt2 = new Label
        {
            Text = "����� �����:",
            Location = new Point(10, 40),
            Size = new Size(80, 20),
            ForeColor = Color.White
        };
        lblInt2.Parent = panelSecondFraction;

        txtInt2.Location = new Point(100, 38);
        txtInt2.Size = new Size(90, 24);
        txtInt2.BackColor = Color.FromArgb(60, 60, 60);
        txtInt2.ForeColor = Color.White;
        txtInt2.BorderStyle = BorderStyle.FixedSingle;
        txtInt2.Parent = panelSecondFraction;
        toolTip.SetToolTip(txtInt2, "������� ����� ����� ����� (��������, 1)");

        var lblFrac2 = new Label
        {
            Text = "������� �����:",
            Location = new Point(10, 70),
            Size = new Size(80, 20),
            ForeColor = Color.White
        };
        lblFrac2.Parent = panelSecondFraction;

        txtFrac2.Location = new Point(100, 68);
        txtFrac2.Size = new Size(90, 24);
        txtFrac2.BackColor = Color.FromArgb(60, 60, 60);
        txtFrac2.ForeColor = Color.White;
        txtFrac2.BorderStyle = BorderStyle.FixedSingle;
        txtFrac2.Parent = panelSecondFraction;
        toolTip.SetToolTip(txtFrac2, "������� ������� ����� (0�999, ��������, 750)");

        // ������ ��� ������
        panelButtons.Location = new Point(20, 140);
        panelButtons.Size = new Size(430, 50);
        panelButtons.Parent = this;

        // ������ ��������
        btnAdd.Text = "�������";
        btnAdd.Location = new Point(10, 10);
        btnAdd.Size = new Size(100, 30);
        btnAdd.FlatStyle = FlatStyle.Flat;
        btnAdd.BackColor = Color.FromArgb(0, 120, 215);
        btnAdd.ForeColor = Color.White;
        btnAdd.FlatAppearance.BorderSize = 1;
        btnAdd.Click += BtnAdd_Click;
        btnAdd.MouseEnter += (s, e) => btnAdd.BackColor = Color.FromArgb(30, 144, 255);
        btnAdd.MouseLeave += (s, e) => btnAdd.BackColor = Color.FromArgb(0, 120, 215);
        btnAdd.Parent = panelButtons;
        toolTip.SetToolTip(btnAdd, "��������� �������� ���� ������");

        btnSubtract.Text = "�������";
        btnSubtract.Location = new Point(120, 10);
        btnSubtract.Size = new Size(100, 30);
        btnSubtract.FlatStyle = FlatStyle.Flat;
        btnSubtract.BackColor = Color.FromArgb(0, 120, 215);
        btnSubtract.ForeColor = Color.White;
        btnSubtract.FlatAppearance.BorderSize = 1;
        btnSubtract.Click += BtnSubtract_Click;
        btnSubtract.MouseEnter += (s, e) => btnSubtract.BackColor = Color.FromArgb(30, 144, 255);
        btnSubtract.MouseLeave += (s, e) => btnSubtract.BackColor = Color.FromArgb(0, 120, 215);
        btnSubtract.Parent = panelButtons;
        toolTip.SetToolTip(btnSubtract, "��������� ��������� ������ ����� �� ������");

        btnMultiply.Text = "��������";
        btnMultiply.Location = new Point(230, 10);
        btnMultiply.Size = new Size(100, 30);
        btnMultiply.FlatStyle = FlatStyle.Flat;
        btnMultiply.BackColor = Color.FromArgb(0, 120, 215);
        btnMultiply.ForeColor = Color.White;
        btnMultiply.FlatAppearance.BorderSize = 1;
        btnMultiply.Click += BtnMultiply_Click;
        btnMultiply.MouseEnter += (s, e) => btnMultiply.BackColor = Color.FromArgb(30, 144, 255);
        btnMultiply.MouseLeave += (s, e) => btnMultiply.BackColor = Color.FromArgb(0, 120, 215);
        btnMultiply.Parent = panelButtons;
        toolTip.SetToolTip(btnMultiply, "��������� ��������� ���� ������");

        btnCompare.Text = "��������";
        btnCompare.Location = new Point(340, 10);
        btnCompare.Size = new Size(100, 30);
        btnCompare.FlatStyle = FlatStyle.Flat;
        btnCompare.BackColor = Color.FromArgb(0, 120, 215);
        btnCompare.ForeColor = Color.White;
        btnCompare.FlatAppearance.BorderSize = 1;
        btnCompare.Click += BtnCompare_Click;
        btnCompare.MouseEnter += (s, e) => btnCompare.BackColor = Color.FromArgb(30, 144, 255);
        btnCompare.MouseLeave += (s, e) => btnCompare.BackColor = Color.FromArgb(0, 120, 215);
        btnCompare.Parent = panelButtons;
        toolTip.SetToolTip(btnCompare, "�������� ��� ����� �� ��������� ����������");

        // ��������� ��� �����������
        lblResultTitle.Text = "����������";
        lblResultTitle.Location = new Point(20, 195);
        lblResultTitle.Size = new Size(100, 20);
        lblResultTitle.Font = new Font(SystemFonts.DefaultFont.FontFamily.Name, 11, FontStyle.Bold);
        lblResultTitle.ForeColor = Color.White;
        lblResultTitle.Parent = this;

        // ������ ��� ����������� � ����������
        panelResult.Location = new Point(20, 220);
        panelResult.Size = new Size(430, 120);
        panelResult.BorderStyle = BorderStyle.FixedSingle;
        panelResult.BackColor = Color.FromArgb(45, 45, 45);
        panelResult.AutoScroll = true;
        panelResult.Parent = this;

        // ����� ����������� ������ ������
        lblResult.Location = new Point(5, 5);
        lblResult.AutoSize = true;
        lblResult.MaximumSize = new Size(410, 0);
        lblResult.ForeColor = Color.White;
        lblResult.Font = new Font(SystemFonts.DefaultFont.FontFamily.Name, 11);
        lblResult.Parent = panelResult;
        toolTip.SetToolTip(lblResult, "���������� �������� ��� ���������");
    }

    private void BtnAdd_Click(object? sender, EventArgs e)
    {
        try
        {
            Fraction f1 = GetFraction(txtInt1, txtFrac1);
            Fraction f2 = GetFraction(txtInt2, txtFrac2);
            Fraction result = f1 + f2;
            lblResult.Text = $"���������: {f1} + {f2} = {result}";
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnSubtract_Click(object? sender, EventArgs e)
    {
        try
        {
            Fraction f1 = GetFraction(txtInt1, txtFrac1);
            Fraction f2 = GetFraction(txtInt2, txtFrac2);
            Fraction result = f1 - f2;
            lblResult.Text = $"���������: {f1} - {f2} = {result}";
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnMultiply_Click(object? sender, EventArgs e)
    {
        try
        {
            Fraction f1 = GetFraction(txtInt1, txtFrac1);
            Fraction f2 = GetFraction(txtInt2, txtFrac2);
            Fraction result = f1 * f2;
            lblResult.Text = $"���������: {f1} * {f2} = {result}";
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnCompare_Click(object? sender, EventArgs e)
    {
        try
        {
            Fraction f1 = GetFraction(txtInt1, txtFrac1);
            Fraction f2 = GetFraction(txtInt2, txtFrac2);
            string comparison = $"���������:\n" +
                               $"f1 == f2: {(f1 == f2 ? "������" : "����")}\n" +
                               $"f1 != f2: {(f1 != f2 ? "������" : "����")}\n" +
                               $"f1 < f2: {(f1 < f2 ? "������" : "����")}\n" +
                               $"f1 > f2: {(f1 > f2 ? "������" : "����")}\n" +
                               $"f1 <= f2: {(f1 <= f2 ? "������" : "����")}\n" +
                               $"f1 >= f2: {(f1 >= f2 ? "������" : "����")}";
            lblResult.Text = comparison;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private Fraction GetFraction(TextBox intBox, TextBox fracBox)
    {
        if (!long.TryParse(intBox.Text, out long integer))
            throw new Exception("������������ ����� �����.");
        if (!ushort.TryParse(fracBox.Text, out ushort fractional) || fractional > 999)
            throw new Exception("������� ����� ������ ���� �� 0 �� 999.");
        return new Fraction(integer, fractional);
    }
}