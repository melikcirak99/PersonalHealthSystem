function ThemeChoice() {
	const page = document.getElementById("page");
	const red = document.getElementById("red-ch");
	const blue = document.getElementById("blue-ch");
	const purple = document.getElementById("purple-ch");
	const defaultt = document.getElementById("default-ch");
	const dark = document.getElementById("dark-ch");

	red.addEventListener("click", function () {
		page.className = `page-red`;
	});
	purple.addEventListener("click", function () {
		page.className = `page-purple`;
	});
	blue.addEventListener("click", function () {
		page.className = `page-blue`;
	});
	defaultt.addEventListener("click", function () {
		page.className = `page-default`;
	});
	dark.addEventListener("click", function () {
		page.className = `page-dark`;
	});
}

ThemeChoice();