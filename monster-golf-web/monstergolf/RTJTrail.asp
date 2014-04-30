<html>
<style>
td { font-family: verdana; font-size:8pt; border: 1px solid #BBBBBB; }
.tableBorder { border: 1px solid #BBBBBB; }
.tdNoBorder { border: 0px solid #BBBBBB; font-size:10pt;}
body { font-family: verdana; font-size:10pt; border: 6px solid #A011DD; }
.BlueFont { font-family: verdana; font-size:9pt; color:#A011DD; font-weight: bold; }
</style>
<body>
<center>
<font class=BlueFont>
<a class=BlueFont href='RTJTrail.asp?show=aaron'>Aaron</a> 
|
<a class=BlueFont href='RTJTrail.asp?show=dewey'>Dewey</a> 
|
<a class=BlueFont href='RTJTrail.asp?show=don'>Don</a> 
|
<a class=BlueFont href='RTJTrail.asp?show=doug'>Doug</a> 
|
<a class=BlueFont href='RTJTrail.asp?show=goose'>Goose</a> 
|
<a class=BlueFont href='RTJTrail.asp?show=jeff'>Jeff</a> 
|
<a class=BlueFont href='RTJTrail.asp?show=raymo'>Raymo</a> 
|
<a class=BlueFont href='RTJTrail.asp?show=steve'>Steve</a> 
|
<a class=BlueFont href='RTJTrail.asp?show=all'>All</a> 
|
<a class=BlueFont href='RTJTrail.asp?show=master'>Master Sheet</a>
|
<a class=BlueFont href='RTJTrail.asp?show=photo'>Photo's</a>
</font>
<br><br>
</center>
<%If Request("show") <> "" Then
	If Request("show") <> "master" And Request("show") <> "photo" Then %>
<table>
<tr>
<td class=tdNoBorder valign=top>
Scores/Points
<table cellspacing=0 cellpadding=3 class=tableBorder>
<tr>
<td> &nbsp;</td><td>Round</td><td align=center width=25>1</td><td align=center width=25>2</td><td align=center width=25>3</td><td align=center width=25>4</td><td align=center width=25>5</td><td align=center width=25>6</td><td align=center width=25>7</td><td align=center width=25>Total</td>
</tr>
<%		If Request("show") = "aaron" Or Request("show") = "all" Then %>
<tr>
<td rowspan=5 valign=top>Aaron</td>
<td nowrap>Score</td>
<td align=center>95</td><td align=center>86</td><td align=center>94</td><td align=center>74</td><td align=center>85</td><td align=center>90</td><td align=center>87</td><td rowspan=2 valign=top align=center>611</td>
</tr>
<tr>
<td nowrap>Stroke Control</td>
<td align=center>94</td><td align=center>86</td><td align=center>90</td><td align=center>N/A</td><td align=center>85</td><td align=center>90</td><td align=center>87</td>
</tr>
<tr>
<td>Par Points</td><td align=center>26</td><td align=center>33</td><td align=center>29</td><td align=center>39</td><td align=center>35</td><td align=center>30</td><td align=center>32</td><td align=center>224</td>
</tr>
<tr>
<td>Skins</td><td align=center>2</td><td align=center>1</td><td align=center>3</td><td align=center>1</td><td align=center>1</td><td align=center>1</td><td align=center>2</td><td align=center>11</td>
</tr>
<tr>
<td>Team Points</td><td align=center>9</td><td align=center>8</td><td align=center>12.5</td><td align=center>12</td><td align=center>12</td><td align=center>9.5</td><td align=center>0</td><td align=center>63</td>
</tr>
<%		End If
		If Request("show") = "dewey" Or Request("show") = "all" Then %>
<tr>
<td rowspan=5 valign=top>Dewey</td><td nowrap>Score</td>
<td align=center>91</td><td align=center>87</td><td align=center>97</td><td align=center>77</td><td align=center>85</td><td align=center>84</td><td align=center>87</td><td rowspan=2 valign=top align=center>608</td>
</tr>
<tr>
<td nowrap>Stroke Control</td>
<td align=center>89</td><td align=center>86</td><td align=center>94</td><td align=center>N/A</td><td align=center>84</td><td align=center>84</td><td align=center>87</td>
</tr>
<tr><td>Par Points</td><td align=center>27</td><td align=center>31</td><td align=center>21</td><td align=center>30</td><td align=center>33</td><td align=center>33</td><td align=center>30</td><td align=center>205</td>
</tr>
<tr>
<td>Skins</td><td align=center>1</td><td align=center>1</td><td align=center>0</td><td align=center>0</td><td align=center>2</td><td align=center>2</td><td align=center>0</td><td align=center>6</td>
</tr>
<tr>
<td>Team Points</td><td align=center>9</td><td align=center>10</td><td align=center>11.5</td><td align=center>7</td><td align=center>6</td><td align=center>11.5</td><td align=center>0</td><td align=center>55</td>
</tr>
<%		End If
		If Request("show") = "don" Or Request("show") = "all" Then %>
<tr>
<td rowspan=5 valign=top>Don</td>
<td nowrap>Score</td>
<td align=center>113</td><td align=center>119</td><td align=center>126</td><td align=center>99</td><td align=center>120</td><td align=center>110</td><td align=center>114</td><td rowspan=2 valign=top align=center>801</td>
</tr>
<tr>
<td nowrap>Stroke Control</td>
<td align=center>111</td><td align=center>118</td><td align=center>123</td><td align=center>N/A</td><td align=center>115</td><td align=center>108</td><td align=center>113</td>
</tr>
<tr>
<td>Par Points</td><td align=center>22</td><td align=center>18</td><td align=center>14</td><td align=center>26</td><td align=center>19</td><td align=center>26</td><td align=center>22</td><td align=center>147</td>
</tr>
<tr>
<td>Skins</td><td align=center>1</td><td align=center>0</td><td align=center>1</td><td align=center>0</td><td align=center>0</td><td align=center>2</td><td align=center>1</td><td align=center>5</td>
</tr>
<tr>
<td>Team Points</td><td align=center>9</td><td align=center>8</td><td align=center>6.5</td><td align=center>6</td><td align=center>6.5</td><td align=center>6.5</td><td align=center>0</td><td align=center>42.5</td>
</tr>
<%		End If
		If Request("show") = "doug" Or Request("show") = "all" Then %>
<tr>
<td rowspan=5 valign=top>Doug</td><td nowrap>Score</td>
<td align=center>78</td><td align=center>79</td><td align=center>86</td><td align=center>69</td><td align=center>87</td><td align=center>79</td><td align=center>79</td><td rowspan=2 valign=top align=center>557</td>
</tr>
<tr>
<td nowrap>Stroke Control</td>
<td align=center>78</td><td align=center>79</td><td align=center>86</td><td align=center>N/A</td><td align=center>87</td><td align=center>79</td><td align=center>79</td>
</tr>
<tr>
<td>Par Points</td><td align=center>33</td><td align=center>32</td><td align=center>25</td><td align=center>32</td><td align=center>24</td><td align=center>32</td><td align=center>32</td><td align=center>210</td>
</tr>
<tr>
<td>Skins</td><td align=center>2</td><td align=center>1</td><td align=center>0</td><td align=center>1</td><td align=center>0</td><td align=center>0</td><td align=center>3</td><td align=center>7</td>
</tr>
<tr>
<td>Team Points</td><td align=center>9</td><td align=center>10</td><td align=center>11.5</td><td align=center>7</td><td align=center>6</td><td align=center>11.5</td><td align=center>0</td><td align=center>55</td>
</tr>
<%		End If
		If Request("show") = "goose" Or Request("show") = "all" Then %>
<tr>
<td rowspan=5 valign=top>Goose</td><td nowrap>Score</td>
<td align=center>111</td><td align=center>111</td><td align=center>119</td><td align=center>88</td><td align=center>97</td><td align=center>94</td><td align=center>98</td><td rowspan=2 valign=top align=center>718</td>
</tr>
<tr>
<td nowrap>Stroke Control</td>
<td align=center>111</td><td align=center>109</td><td align=center>115</td><td align=center>N/A</td><td align=center>97</td><td align=center>94</td><td align=center>98</td>
</tr>
<tr>
<td>Par Points</td><td align=center>20</td><td align=center>23</td><td align=center>20</td><td align=center>35</td><td align=center>35</td><td align=center>38</td><td align=center>36</td><td align=center>207</td>
</tr>
<tr>
<td>Skins</td><td align=center>0</td><td align=center>1</td><td align=center>3</td><td align=center>0</td><td align=center>2</td><td align=center>3</td><td align=center>3</td><td align=center>12</td>
</tr>
<tr>
<td>Team Points</td><td align=center>9</td><td align=center>8</td><td align=center>12.5</td><td align=center>12</td><td align=center>12</td><td align=center>9.5</td><td align=center>0</td><td align=center>63</td>
</tr>
<%		End If
		If Request("show") = "jeff" Or Request("show") = "all" Then %>
<tr>
<td rowspan=5 valign=top>Jeff</td><td nowrap>Score</td>
<td align=center>109</td><td align=center>100</td><td align=center>97</td><td align=center>67</td><td align=center>89</td><td align=center>84</td><td align=center>0</td><td rowspan=2 valign=top align=center>546</td>
</tr>
<tr>
<td nowrap>Stroke Control</td>
<td align=center>83</td><td align=center>91</td><td align=center>96</td><td align=center>N/A</td><td align=center>88</td><td align=center>83</td><td align=center>0</td>
</tr>
<tr>
<td>Par Points</td><td align=center>22</td><td align=center>22</td><td align=center>16</td><td align=center>36</td><td align=center>25</td><td align=center>29</td><td align=center>0</td><td align=center>150</td>
</tr>
<tr>
<td>Skins</td><td align=center>1</td><td align=center>0</td><td align=center>1</td><td align=center>0</td><td align=center>0</td><td align=center>0</td><td align=center>0</td><td align=center>2</td>
</tr>
<tr>
<td>Team Points</td><td align=center>9</td><td align=center>10</td><td align=center>5.5</td><td align=center>11</td><td align=center>11.5</td><td align=center>8.5</td><td align=center>0</td><td align=center>55.5</td>
</tr>
<%		End If
		If Request("show") = "raymo" Or Request("show") = "all" Then %>
<tr>
<td rowspan=5 valign=top>Raymo</td><td nowrap>Score</td>
<td align=center>132</td><td align=center>122</td><td align=center>123</td><td align=center>108</td><td align=center>113</td><td align=center>119</td><td align=center>0</td><td rowspan=2 valign=top align=center>717</td>
</tr>
<tr>
<td nowrap>Stroke Control</td>
<td align=center>122</td><td align=center>118</td><td align=center>123</td><td align=center>N/A</td><td align=center>112</td><td align=center>117</td><td align=center>0</td>
</tr>
<tr>
<td>Par Points</td><td align=center>14</td><td align=center>21</td><td align=center>16</td><td align=center>26</td><td align=center>26</td><td align=center>18</td><td align=center>0</td><td align=center>121</td>
</tr>
<tr>
<td>Skins</td><td align=center>0</td><td align=center>0</td><td align=center>0</td><td align=center>1</td><td align=center>1</td><td align=center>0</td><td align=center>0</td><td align=center>2</td>
</tr>
<tr>
<td>Team Points</td><td align=center>9</td><td align=center>8</td><td align=center>6.5</td><td align=center>6</td><td align=center>6.5</td><td align=center>6.5</td><td align=center>0</td><td align=center>42.5</td>
</tr>
<%		End If
		If Request("show") = "steve" Or Request("show") = "all" Then %>
<tr>
<td rowspan=5 valign=top>Steve</td>
<td nowrap>Score</td>
<td align=center>86</td><td align=center>90</td><td align=center>101</td><td align=center>73</td><td align=center>86</td><td align=center>80</td><td align=center>87</td><td rowspan=2 valign=top align=center>603</td>
</tr>
<tr>
<td nowrap>Stroke Control</td>
<td align=center>86</td><td align=center>88</td><td align=center>95</td><td align=center>N/A</td><td align=center>86</td><td align=center>79</td><td align=center>85</td>
</tr>
<tr>
<td>Par Points</td><td align=center>31</td><td align=center>26</td><td align=center>20</td><td align=center>34</td><td align=center>31</td><td align=center>37</td><td align=center>31</td><td align=center>210</td>
</tr>
<tr>
<td>Skins</td><td align=center>1</td><td align=center>0</td><td align=center>1</td><td align=center>1</td><td align=center>1</td><td align=center>1</td><td align=center>0</td><td align=center>5</td>
</tr>
<tr>
<td>Team Points</td><td align=center>9</td><td align=center>10</td><td align=center>5.5</td><td align=center>11</td><td align=center>11.5</td><td align=center>8.5</td><td align=center>0</td><td align=center>55.5</td>
</tr>
<%		End If %>
</table>
</td>
<td valign=top class=tdNoBorder>
Course Ratings and Slope
<table cellspacing=0 cellpadding=1 class=tableBorder>
<tr><td>Round</td><td>Course</td><td>Rating</td><td>Slope</td></tr>
<tr><td align=center>1</td><td>Capitol Hill - Senator</td><td align=center>72.4</td><td align=center>125</td></tr>
<tr><td align=center>2</td><td>Capitol Hill - Legislator</td><td align=center>70.8</td><td align=center>124</td></tr>
<tr><td align=center>3</td><td>Cambrian Ridge - Sherling/Canyon</td><td align=center>73.2</td><td align=center>137</td></tr>
<tr><td align=center>4</td><td>Cambrian Ridge - Loblolly/Short</td><td align=center>N/A</td><td align=center>N/A</td></tr>
<tr><td align=center>5</td><td>Grand National - Lake</td><td align=center>72.3</td><td align=center>134</td></tr>  
<tr><td align=center>6</td><td>Grand National - Links</td><td align=center>70.2</td><td align=center>131</td></tr>
<tr><td align=center>7</td><td>Capitol Hill - Judge</td><td align=center>69.3</td><td align=center>129</td></tr>
</table>
</td>
</tr>
</table>
<%	ElseIf Request("show") = "master" Then %>
Master Sheet
<table cellspacing=0 cellpadding=1 class=tableBorder>
<tr>
	<td align=center> &nbsp;</td><td align=center>Seed</td><td align=center>Skins1</td><td align=center>Skins2</td><td align=center>Team1</td><td align=center>Team2</td><td align=center>Skins3</td><td align=center>Skins4</td><td align=center>Team3</td><td align=center>Team4</td>
	<td align=center>Skins5</td><td align=center>Skins6</td><td align=center>Team5</td><td align=center>Team6</td><td align=center>Skins7</td><td align=center>ParPs</td><td align=center>TeamTot<td align=center>Cars</td><td align=center>sides</td><td align=center>Get/Owe</td>
</tr>
<tr>
	<td>Aaron</td><td align=center>-75</td><td align=center>10</td><td align=center>10</td><td align=center>7.5</td><td align=center>0</td><td align=center>17</td><td align=center>10</td><td align=center>15</td><td align=center>15</td>
	<td align=center>1</td><td align=center>-1</td><td align=center>15</td><td align=center>15</td><td align=center>3</td><td align=center>150</td><td align=center>20</td><td align=center>-60</td><td align=center>1</td><td align=center>153.5</td>
</tr>
<tr>
	<td>Dewey</td><td align=center>-75</td><td align=center>0</td><td align=center>10</td><td align=center>7.5</td><td align=center>15</td><td align=center>-10</td><td align=center>-10</td><td align=center>15</td>
	<td align=center>0</td><td align=center>14</td><td align=center>8</td><td align=center>0</td><td align=center>15</td><td align=center>-10</td><td align=center>0</td><td align=center>0</td><td align=center>-60</td><td align=center>1</td><td align=center>-79.5</td>
</tr>
<tr>
	<td>Don</td><td align=center>-75</td><td align=center>0</td><td align=center>-10</td><td align=center>7.5</td><td align=center>0</td><td align=center>-1</td><td align=center>-10</td><td align=center>0</td><td align=center>0</td>
	<td align=center>-10</td><td align=center>8</td><td align=center>0</td><td align=center>0</td><td align=center>-3</td><td align=center>0</td><td align=center>0</td><td align=center>0</td><td align=center>14</td><td align=center>-79.5</td>
</tr>
<tr>
	<td>Doug</td><td align=center>-75</td><td align=center>10</td><td align=center>10</td><td align=center>7.5</td><td align=center>15</td><td align=center>-10</td><td align=center>10</td><td align=center>15</td><td align=center>0</td>
	<td align=center>-10</td><td align=center>-10</td><td align=center>0</td><td align=center>15</td><td align=center>10</td><td align=center>25</td><td align=center>0</td><td align=center>-60</td><td align=center>-22</td><td align=center>-69.5</td>
</tr>
<tr>
	<td>Goose</td><td align=center>-75</td><td align=center>-10</td><td align=center>10</td><td align=center>7.5</td><td align=center>0</td><td align=center>17</td><td align=center>-10</td><td align=center>15</td><td align=center>15</td>
	<td align=center>12</td><td align=center>16</td><td align=center>15</td><td align=center>15</td><td align=center>10</td><td align=center>0</td><td align=center>20</td><td align=center>-50</td><td align=center>26</td><td align=center>33.5</td>
</tr>
<tr>
	<td>Jeff</td><td align=center>-75</td><td align=center>0</td><td align=center>-10</td><td align=center>7.5</td><td align=center>15</td><td align=center>-2</td><td align=center>-10</td><td align=center>0</td><td align=center>15</td>
	<td align=center>-10</td><td align=center>-10</td><td align=center>15</td><td align=center>0</td><td align=center>0</td><td align=center>0</td><td align=center>0</td><td align=center>170</td><td align=center>-14</td><td align=center>91.5</td>
</tr>
<tr>
	<td>Raymo</td><td align=center>-75</td><td align=center>-10</td><td align=center>-10</td><td align=center>7.5</td><td align=center>0</td><td align=center>-10</td><td align=center>10</td><td align=center>0</td><td align=center>0</td>
	<td align=center>2</td><td align=center>-10</td><td align=center>0</td><td align=center>0</td><td align=center>0</td><td align=center>0</td><td align=center>0</td><td align=center>-50</td><td align=center>0</td><td align=center>-145.5</td>
</tr>
<tr>
	<td>Steve</td><td align=center>-75</td><td align=center>0</td><td align=center>-10</td><td align=center>7.5</td><td align=center>15</td><td align=center>-1</td><td align=center>10</td><td align=center>0</td><td align=center>15</td>
	<td align=center>1</td><td align=center>-1</td><td align=center>15</td><td align=center>0</td><td align=center>-10</td><td align=center>25</td><td align=center>0</td><td align=center>110</td><td align=center>-6</td><td align=center>95.5</td>
</tr>
<tr>
	<td>Totals</td><td align=center>-600</td><td align=center>0</td><td align=center>0</td><td align=center>60</td><td align=center>60</td><td align=center>0</td><td align=center>0</td><td align=center>60</td><td align=center>60</td>
	<td align=center>0</td><td align=center>0</td><td align=center>60</td><td align=center>60</td><td align=center>0</td><td align=center>200</td><td align=center>40</td><td align=center>0</td><td align=center>0</td><td align=center>0</td>
</tr>
</table>
<%	Else %>
<center>
<img src='RTJTrail/02.jpg'>
<br>
<img src='RTJTrail/03.jpg'>
<br>
<img src='RTJTrail/04.jpg'>
<br>
<img src='RTJTrail/05.jpg'>
<br>
<img src='RTJTrail/06.jpg'>
<br>
<img src='RTJTrail/07.jpg'>
<br>
<img src='RTJTrail/08.jpg'>
<br>
<img src='RTJTrail/09.jpg'>
<br>
<img src='RTJTrail/10.jpg'>
<br>
<img src='RTJTrail/11.jpg'>
<br>
<img src='RTJTrail/12.jpg'>
<br>
</center>
<%	End If
End If %>
</body>
</html>