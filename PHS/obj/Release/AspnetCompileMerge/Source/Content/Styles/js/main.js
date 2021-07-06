var password = ["A","B","C","D","F","G","Ğ","ğ","ü","@","£","$","{","}","]","]","#","Ü","H","*","J","K","-","!","%","&","=","?","_","L","a","Ş","Q","W","E","R","T","Y","U","İ","O","P","Z","X","C","V","B","N","M","","*","-","+","a","s","d","f","g","h","k","l","i","z","x","c","v","b","n","m","m","q","w","e","r","t","y","u","o","p",",",".","*","<",">","1","2","3","4","5","6","7","8","9","0",",",".","*","<",">","1","2","3","4","5","6","7","8","9","0"];
var button = document.getElementById("recommended-password-btn");


function ParolaGosterGizle(_icon, _input, _tip) {
	_icon = document.getElementById(_icon);
	_input = document.getElementById(_input);
	_tip = document.getElementById(_tip);
	_icon.className = (_icon.className == 'far fa-eye' ? 'far fa-eye-slash' : 'far fa-eye');
	_input.type = (_input.type == 'password' ? 'text' : 'password');
	_tip.innerText = (_input.type == 'password' ? 'Şifreyi Göster' : 'Şifreyi Gizle');
}



function GucluSifreOner(_input){
	_input = document.getElementById(_input);
	_input.value = "";
	for (i = 0; i <= 14; i++){
		_input.value += password[Math.floor(Math.random() * 108)];

	}
}


function VucutKitleIndeksiHesapla(){
	boy       = document.getElementById("boy").value;
	kilo      = document.getElementById("kilo").value;
	var angry = `<i class="far fa-angry"></i>`;
	var happy = `<i class="far fa-smile-wink"></i>`;
	var sad   = `<i class="fas fa-frown-open"></i>`;

	boy  = Number(boy)/100;
	kilo = Number(kilo);
	var Vki = kilo / (boy*boy);

	if (Vki<=18.5) {
		document.getElementById("vki-sonuc").innerHTML = Vki.toFixed(1) + `<span class="text-danger text-capitalize"> düşük kilolu ${angry}</span>`;
	}
	else if(Vki<=24.9 && Vki>18.5){
		document.getElementById("vki-sonuc").innerHTML = Vki.toFixed(1) + `<span class="text-success text-capitalize"> normal kilolu ${happy}</span>`;
	}
	else if(Vki<=29.9 && Vki>25){
		document.getElementById("vki-sonuc").innerHTML = Vki.toFixed(1) + `<span class="text-warning text-capitalize"> fazla kilolu ${sad}</span>`;;
	}
	else if(Vki<=40 && Vki>30){
		document.getElementById("vki-sonuc").innerHTML = Vki.toFixed(1) + `<span class="text-danger text-capitalize"> obez ${angry}</span>`;
	}
	else if(Vki>=40){
		document.getElementById("vki-sonuc").innerHTML = Vki.toFixed(1) + `<span class="text-danger text-capitalize"> aşırı obez ${angry}</span>`;
    }
    if (boy == 0 || kilo == 0 || boy <= 0 || kilo <= 0 ) {
        document.getElementById("vki-sonuc").innerHTML = `<span class="text-danger text-capitalize">lütfen doğru giriş yapın</span>`;
    }

}

function Temizle(){
	document.getElementById("boy").value  = "";
	document.getElementById("kilo").value = "";
	document.getElementById("vki-sonuc").innerText = "";

}

/* giriş sayfası input animasyonu*/


function GirisAnimation(_span, _textbox) {
	_span = document.getElementById(_span);
	_textbox = document.getElementById(_textbox);

	_span.className = "placeholder-up";
	_textbox.placeholder = "";
}
function GirisAnimationFout(_span, _textbox) {
	_span = document.getElementById(_span);
	_textbox = document.getElementById(_textbox);

	if (_textbox.value == "") {
		_span.className = "placeholder";
	}
}

var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
	return new bootstrap.Tooltip(tooltipTriggerEl)
})