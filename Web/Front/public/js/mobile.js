$(document).ready(function(){window.addEventListener("load",function(){setTimeout(function(){window.scrollTo(0,0);});});$('.quicknav').click(function(){$('#sidebar').toggle();$('#content').toggleClass('content-mobile');if($('#sidebar').is(":hidden")){$('#header').css("position",'static');$('#content').css("padding-top",'0');}else{$('#header').css("position",'fixed');$('#content').css("padding-top",'70px');}});if(window.devicePixelRatio==2){var images=$("img.2x");for(var i=0;i<images.length;i++){var imageType=images[i].src.substr(-4);var imageName=images[i].src.substr(0,images[i].src.length-4);imageName+="@2x"+imageType;images[i].src=imageName;}}});