
			
	function makearray(n) {
	this.length = n;
	for (var i = 1; i <= n; i++) this[i] = 0;
	return this;}

	
var asciitable = new makearray (85);
asciitable.length=85;
for (var i=0;i<=85;i++) asciitable[i]=" ";
//special Characters group
asciitable[0]="!"; asciitable[1]=";"; 
asciitable[2]="#"; asciitable[3]="$";
asciitable[4]="%"; asciitable[5]="&"; 
asciitable[6]="'"; asciitable[7]="(";
asciitable[8]=")"; asciitable[9]="*"; 
asciitable[10]="+"; asciitable[11]=",";
asciitable[12]="-"; asciitable[13]="."; 
asciitable[14]="/"; asciitable[15]="["; 
asciitable[16]="]"; asciitable[17]="^"; 
asciitable[18]="_"; asciitable[19]="`";
asciitable[20]=":"; asciitable[21]="?"; 
asciitable[22]="@";
// numbers group
asciitable[23]="0"; asciitable[24]="1"; 
asciitable[25]="2"; asciitable[26]="3"; 
asciitable[27]="4"; asciitable[28]="5"; 
asciitable[29]="6"; asciitable[30]="7"; 
asciitable[31]="8"; asciitable[32]="9";
// uppercase Chars  
asciitable[33]="A"; asciitable[34]="B"; 
asciitable[35]="C"; asciitable[36]="D";
asciitable[37]="E"; asciitable[38]="F"; 
asciitable[39]="G"; asciitable[40]="H";
asciitable[41]="I"; asciitable[42]="J"; 
asciitable[43]="K"; asciitable[44]="L";
asciitable[45]="M"; asciitable[46]="N"; 
asciitable[47]="O"; asciitable[48]="P";
asciitable[49]="Q"; asciitable[50]="R"; 
asciitable[51]="S"; asciitable[52]="T";
asciitable[53]="U"; asciitable[54]="V"; 
asciitable[55]="W"; asciitable[56]="X";
asciitable[57]="Y"; asciitable[58]="Z";
// lower case Chars 
asciitable[59]="a"; asciitable[60]="b"; 
asciitable[61]="c"; asciitable[62]="d";
asciitable[63]="e"; asciitable[64]="f"; 
asciitable[65]="g"; asciitable[66]="h";
asciitable[67]="i"; asciitable[68]="j"; 
asciitable[69]="k"; asciitable[70]="l";
asciitable[71]="m"; asciitable[72]="n"; 
asciitable[73]="o"; asciitable[74]="p";
asciitable[75]="q"; asciitable[76]="r"; 
asciitable[77]="s"; asciitable[78]="t";
asciitable[79]="u"; asciitable[80]="v"; 
asciitable[81]="w"; asciitable[82]="x";
asciitable[83]="y"; asciitable[84]="z"; 

function nchar(num) {
	if ((num>=0) && (num<=84)) return asciitable[num];
}
	
    
	function getRandomChars(_charsRange, _length, _target) {
		var _arr = [];
		var l1 = _charsRange[0];
		var rest = _charsRange[1] - _charsRange[0] + 1;
		// 
		while (_arr.length < _length) {
			var charCode = Math.floor(Math.random() * rest);
			// 
			var symbol = nchar(charCode + l1);
			// adds symbol only if it's unique
			if (_arr.toString().indexOf(symbol) == -1)
				_arr[_arr.length] = symbol;
		}
		//
		for (var i = 0; i < _length; i++)
			_target[_target.length] = _arr[i];
	}
	
	function mixCharArray(_arr) {
		var _str = "";
		//
		while (_arr.length > 0){
			// get random element
			var index = Math.floor(Math.random() * (_arr.length - 1));
			// add element
			_str += _arr[index];
			// remove element from array
			var _tmp = [];
			for (var i = 0; i < _arr.length; i++) {
				if (i != index)
					_tmp[_tmp.length] = _arr[i];
			}
			_arr = _tmp;
		}
		//
		return _str;
	}
	
	function GeneratePassword(_maxLength, _Upper, _Number, _Special, txt1, txt2)
	{
	    // If the length is greater than 26 then and no Uppercase and Numbers and Symbols are required then set the max length to 26
	    if ((_maxLength > 26) && (_Upper == 0) && (_Number == 0) && (_Special == 0)) _maxLength = 26;
	    if (_maxLength == 0)
	    {
	         alert("Your password length is set to 0. Please check your SolidCP Policy");
	         _Upper = 0;
	         _Number = 0;
	         _Special = 0;
	    }   
	    var pass = "";
	    var pas_chars = [];
	    var _Lower = _maxLength - _Upper - _Number - _Special;
	    while (_Lower > 26) {
	        if (_Upper > 0) _Upper++;
	        if (_Number > 0) _Number++;
	        if (_Special > 0) _Special++;
	        _Lower = _maxLength - _Upper - _Number - _Special;
	    }
		getRandomChars([0, 22], _Special, pas_chars);
		getRandomChars([23, 32], _Number, pas_chars);
		getRandomChars([33, 58], _Upper, pas_chars);
		getRandomChars([59, 84], _Lower, pas_chars);
	
		pass = mixCharArray(pas_chars);
		
		document.getElementById(txt1).value = pass;
		document.getElementById(txt2).value = pass;
	}	
	