   M S B A M L   `   `   `  ���� D  @ExamMaker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:ExamMaker	ExamMaker      ExamMaker.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     )localclr-namespace:ExamMaker     -     � Title$  Welcome to Exam Maker��$	��727��$
��1064�� � WindowStartupLocation$ CenterScreen=� � 	Resources     ExamMaker.CreateValidation   myValidation  SubjectErrorTemplate �� 
TargetType �� Triggers P� Property P� Value �� Property �� Value	 �� RelativeSource
 �� Path��%�  &	        &	      '�� r     '�� �       �� .R�]� ��. .+��� $��Right=�
���  ��$
��12pt(�$
��Bold"�*�� 
���  ��$��2q�._��� �� " �� P�  �@=�HasError . . ���  �@�ToolTip �� 	 �� *Self+$)
 #(Validation.Errors)[0].ErrorContent��� " �� P�  �@=�HasError . . ���  �@�ToolTip �� 	 �� *Self+$)
 #(Validation.Errors)[0].ErrorContent�.��� $��0,0,22,9q�.+�]� ��.  ��%   'Q�       �� " Q�.�-   .+�R� -    R�Name$ QuizTree��$
��Left=�$	��400��$��
10,200,0,0q�$	��Top=�$	��225�� J� RenderTransformOrigin$ 0.489,0.587&����� " Q�.���  �@Q�
IsExpanded �� *
IsExpanded+ �� Mode$ TwoWay=���  �@Q�
IsSelected �� *
IsSelected+$ TwoWay=���  � ��$ Normal"� P�  �@Q�
IsSelected . . ���  � ��$
 Bold"��� $	��150��$	��Top=�$"��Pictures/RobertsonBanner.jpg��$��241,0,-26,0q�� -    �Name$ MainGrid��$
��Left=�$	��450��$��241,152,0,0q�$	��Top=�$	��764�� � ColumnDefinitions �� $
��275* ��� $
��504* �.+��� -   $��Title��$
��Left=�$��5,10,0,0q�$��30(����#F62217$
��Wrap=�$&�� Welcome to Robertson Quiz Maker!��$	��Top=�$ 0.476,0.526&�$��2��� -   $ gridQuizSummary��$y�Hidden=�$
��Left=�$	��170��$��	15,45,0,0q�$	��Top=�$	��749��$ 0.497,0.323&�$��2��.+��� $��Subject:��$��14(�$
��Left=�$��
314,48,0,0q�$	��Top=��� -   $��
txtSubject��$
��Left=�$��23��$��
389,51,0,0q� �� TextWrapping$
 Wrap=�$	��Top=�$	��113��$ -1.956,2.217&� =� ErrorTemplate# [ .������  �� Source# [  $
 Subject� �� UpdateSourceTrigger$ Explicit=� �� ValidationRules 5  /System.Windows.Controls.ExceptionValidationRule  .���� $��Time in Minutes:��$��14(�$
��Left=�$��
537,48,0,0q�$	��Top=��� -    ��Name$ txtTime��$
��Left=�$��23��$��
654,51,0,0q�$
 Wrap=�$	��Top=�$��66��# [ .������ # [  $

 Time�$ PropertyChanged=�   .���� $�� Quiz Title:��$��14(�$
��Left=�$��	39,48,0,0q�$	��Top=��� -	   $ txtTitle��$
��Left=�$��23��$��
119,51,0,0q�$
 Wrap=�$	��Top=�$	��163��# [ .������ # [  $
 Title�$ Explicit=�   .���� $��Course:��$��14(�$
��Left=�$��	41,77,0,0q�$	��Top=�$ 0.421,0.115&�$��59���� -
    ��Name$ 	cmbCourse��$
��Left=�$��
119,84,0,0q�$	��Top=�$	��163�� �� SelectedValuePath$ Content��# [  �� SelectedValue �� # [  $
 Course�$ Explicit=�   .@��� $��Software Developer��$
��Left=�$	��200���� $��Difficulty:��$��14(�$
��Left=�$��
314,75,0,0q�$	��Top=��� -   $ cmbDiff��$
��Left=�$��
389,84,0,0q�$	��Top=�$	��120��$ Content�� �� # [  $
 
Difficulty�$ Explicit=�   .@��� $��Beginner��$
��Left=�$��95���� $��Intermediate��$
��Left=�$��95���� $��Advance��$
��Left=�$��95���� $��Quiz Summary��
�����p�$
��Bold"�$��16(�$
��Left=�$��	304,0,0,0q�$	��Top=�$	��215���� $��Total items:��$
��Left=�$��
549,82,0,0q�$	��Top=�$��80���� -    ��Name$ QuizItemCount��$��0��$
��Left=�$��
629,82,0,0q�$	��Top=��� -    ��Name$ 	btnAddNew��$	��Add��$
��Left=�$��249,125,0,0q�$	��Top=�$��75��#��� ���� $��Click to Add New Question��$��14(�$
��Left=�$��
49,120,0,0q�$	��Top=�$ -0.132,0.769&�$	��180��� -   $ GridQuestionType��$y�Hidden=�$
��Left=�$��32��$��100,163,0,0q�$	��Top=�$	��340��$ 0.529,0.531&�$��1�� �� $
��275* ��� $
��504* �.+��� $��Select Question Type:��$��14(�$
��Left=�$��20,4,0,0q�$	��Top=�$��2��$	��143���� -   $ cmbQuestionType��$
��Left=�$��55,4,0,0q�$	��Top=�$ 4.036,0.682&�$	��140��$ Content��$��1��.@��� 1��$��Multiple Choice��$
��Left=�$��95���� 2��$��Fill in the blanks��$
��Left=�$��95���� 3��$��
True False��$
��Left=�$	��110���� 4��$��Matching Type��$
��Left=�$��95���� 5��$��Long Answer��$
��Left=�$��95��� -   $ gridEditDelete��$y�Hidden=�$��1��$
��Left=�$	��100��$��340,293,0,0q�$	��Top=�$	��100��.+��� -   $ btnEdit��$
��Edit��$
��Left=�$��	15,10,0,0q�$	��Top=�$��75��#��� ���� -   $ 	btnDelete��$��Delete��$
��Left=�$��	15,55,0,0q�$	��Top=�$��75��#��� ��� -   $ gridFillBlanks��$y�Hidden=�$
��Left=�$	��195��$��
29,235,0,0q�$	��Top=�$	��575��$ 	0.5,0.258&�$��2��.+��� $ ��Fill Blanks Question Text:��$��14(�$
��Left=�$��10,0,0,0q�$	��Top=�$	��170���� -   $ txtFillBlanks��$
 Wrap=�$
��Left=�$��23��$��	50,34,0,0q�$�� ��$	��Top=�$	��496���� -   $ 
txtChoice1��$
��Left=�$��23��$��
135,68,0,0q�$
 Wrap=�$�� ��$	��Top=�$	��120���� -   $ 
txtChoice2��$
��Left=�$��23��$��135,112,0,0q�$
 Wrap=�$�� ��$	��Top=�$	��120���� -   $ 
txtChoice3��$
��Left=�$��23��$��
360,68,0,0q�$
 Wrap=�$�� ��$	��Top=�$	��120��$ 0.917,0.652&��� -   $ 
txtChoice4��$
��Left=�$��23��$��360,109,0,0q�$
 Wrap=�$�� ��$	��Top=�$	��120��$ 2.208,0.304&�	� -    	�Name$ 
cboChoice1��$��1.��$
��Left=�$��	90,76,0,0q�$	��Top=�	� -   $ 
cboChoice2��$��2.��$
��Left=�$��
316,76,0,0q�$	��Top=�	� -   $ 
cboChoice3��$��3.��$
��Left=�$��
90,120,0,0q�$	��Top=�	� -   $ 
cboChoice4��$��4.��$
��Left=�$��316,117,0,0q�$	��Top=��� -   $ btnFillBlanks��$��Submit Question��$
��Left=�$��245,160,0,0q�$	��Top=�$	��120��$��25��� -   $ gridTrueFalse��$y�Visible=�$
��Left=�$	��160��$��
39,245,0,0q�$	��Top=�$	��555��$ 
0.485,0.35&�$��2��.+��� $"��True or False Question Text:��$��14(�$
��Left=�$��10,6,0,0q�$	��Top=�	� -   $ rbTrue��$
��True��$
��Left=�$��
165,75,0,0q�$	��Top=�	� -    $ rbFalse��$��False��$
��Left=�$��
355,75,0,0q�$	��Top=��� -!   $ txtTrueFalse��$
��Left=�$��23��$��	45,35,0,0q�$
 Wrap=�$�� ��$	��Top=�$	��480���� -"   $ btnTrueFalse��$��Submit Question��$
��Left=�$��225,105,0,0q�$	��Top=�$	��115��$��30��� -#   $ gridMultipleChoice��$y�Hidden=�$
��Left=�$	��210��$��
15,240,0,0q�$	��Top=�$	��564��$ 0.42,-0.044&�$��2��.+��� $$��Multiple Choice Question Text:��$��14(�$
��Left=�$��10,0,0,0q�$	��Top=�$ -0.395,0.192&�$	��215��$��30���� -$   $ 
txtOption1��$
��Left=�$��23��$��	84,88,0,0q�$
 Wrap=�$	��Top=�$	��190��# [ .������ # [  $
 Option1�$ Explicit=�   .��	� -%   $ 	rbOption1��$��1.��$
��Left=�$��	36,91,0,0q�$	��Top=�	� -&   $ 	rbOption2��$��2.��$
��Left=�$��
36,132,0,0q�$	��Top=��� -'   $ 
txtOption2��$
��Left=�$��23��$��
84,132,0,0q�$
 Wrap=�$	��Top=�$	��190��$ 
0.51,0.348&�# [ .������ # [  $
 Option2�$ Explicit=�   .��	� -(   $ 	rbOption3��$��3.��$
��Left=�$��
309,91,0,0q�$	��Top=�	� -)   $ 	rbOption4��$��4.��$
��Left=�$��309,135,0,0q�$	��Top=��� -*   $ 
txtOption3��$
��Left=�$��23��$��
354,88,0,0q�$
 Wrap=�$	��Top=�$	��190��# [ .������ # [  $
 Option3�$ Explicit=�   .���� -+   $ 
txtOption4��$
��Left=�$��23��$��354,132,0,0q�$
 Wrap=�$	��Top=�$	��190��# [ .������ # [  $
 Option4�$ Explicit=�   .���� -,   $ txtQuestion��$
��Left=�$��23��$��	24,45,0,0q�$
 Wrap=�$	��Top=�$	��505��# [ .������ # [  $
 MultiQuestion�$ Explicit=�   .���� --   $ 	btnSubmit��$��Submit Question��$
��Left=�$��284,175,0,0q�$	��Top=�$	��120��$ 0.431,-0.233&�$��25��o� -.    o�Name$ MenuBar��$��25��$��	0,0,-26,0q�$	��Top=�.@�m� -/    m�Name$ FileMenu��$��_File��.@�m� -0   $	 New��$��_New...���� m� -1   $
 Open��$��_Open...���� m� -2   $
 Save��$��_Save��{�.  m� -3   $ Save_As��$��
_Save As..��{�.  �� m� -4   $
 Exit��$��_Exit��m� -5   $
 View��$��_View��.@�m� -6   $ ErrorLog��$��_Error Log...��m� -7   $
 Help��$��_Help��.@�m� -8   $ HowTo��$��
_How To...���� $��Quiz Tree view:��$��14(�$
��Bold"�
�����p�$
��Left=�$��
20,165,0,0q�$	��Top=�$	��110���� -9   $ status��$
��Left=�$��22��$��
10,655,0,0q�$
 Wrap=�$��status��$	��Top=�$
��1014�� �� 
IsReadOnly . $
��Bold"�