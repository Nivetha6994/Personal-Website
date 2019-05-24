// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.
function change(x) {
    x.src = "images/Profile1.jpg";
}
function changeback(x) {
    x.src = "images/Profile.jpg";
}
function changecolor(x) {
    x.style.color = '#d04261';
}
function changecolor1(x) {
    x.style.color = '#4cd038';
}
function changecolorback(x) {
    x.style.color = '#4ca7ad';
}
function IntroFunc() {
    var x = document.getElementsByClassName("Intro");
    x[0].innerHTML =

       ` <h3>About Me</h3>
        <p onmouseover="changecolor1(this)" onmouseout="changecolorback(this)">Hello/Hola/Namaste/Vanakkam!</p>
        <p id="intro" onmouseover="changecolor(this)" onmouseout="changecolorback(this)">
            I am Nivetha Ramachandran. I am a native of the vibrant nation called India.
            Currently, I reside in the United States of America, where I have come to fulfill
            my dreams and aspirations. I am passionate about development, coding or basically
            anything that I find challenging and gives a tough challenge to my gray cells!
            Also, thank you for visiting my page. Please go through all the sections and
            do login and provide your feedback!
                </p>`
}
