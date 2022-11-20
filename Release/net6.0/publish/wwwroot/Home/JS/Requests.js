async function getdata() {
		
    // отправляет запрос и получаем ответ
    const response = await fetch("/Home/Data", {
        method: "GET",
        headers: { "Accept": "application/json" }
    });
    // если запрос прошел нормально
    if (response.ok) {
        //создание форм из документа
        

        var message = await response.json();

        var postname = message.PostName;
        var nicedayusername = message.nicedayusername;
        

        var table = document.getElementById("TableContent");
        var infotable = document.getElementById("infotable");
        for (let i = 0; i < message.postID.length; i++)
        {
            var row = table.insertRow(i);
           
            
            getPosts(message, i, row)
        }
        {
            var inforow= infotable.insertRow(0);
            inforow.innerHTML = "<p>" + nicedayusername + "</p>"
            var inforow= infotable.insertRow(1);
            
        }


    }

}
async function getPosts(message, i, row)
{
    const response = await fetch("/GetPost/GetPosts/" + message.postID[i], {
        method: "GET",
        headers: { "Accept": "application/json" }
    });
    // если запрос прошел нормально
    if (response.ok) {
        var contentpost = await response.text();
        
        row.innerHTML = "<td id=\""+ message.postID[i] +"\" class=\"contentrow\"><a id=\"titlecontentlink\" href=\"content/index/" + message.postID[i]+"\">"+message.postName[i]+"</a>  "+contentpost+" </td>";
        getlikeofpost(message.postID[i]);
    }




}
async function getlikeofpost(id)
{
        const response = await fetch("/requests/users", {
        method: "POST",
        headers: { "Accept": "application/json", "Content-Type": "application/json" },
        body: JSON.stringify({
                ID : id
            })
    });
    // если запрос прошел нормально
    if (response.ok) {
        //выбор формы
    var assetsofcontent = await response.json();
    
     var element = document.createElement("div");
     element.id = "aftercontent";
        //div id = aftercontent
     var likecount = document.createElement("input");
     likecount.id = "likecount/" + id;
     likecount.className = "LikesCount";
     likecount.value =assetsofcontent.likes + " likes";
     likecount.disabled = true;
     element.append(likecount);
     //element.appendChild(document.createTextNode(assetsofcontent.likes + " likes"));
     //кнопка лайка
     var likebutton = document.createElement("button");
     likebutton.id = "likebutton/" + id;
     likebutton.className = "LikesButton";
     likebutton.append(assetsofcontent.btn);
     //likebutton.addEventListener("click", async() => await getUser(user.id));
     likebutton.addEventListener("click", async() => await addlikes(id))
     element.append(likebutton);
     
    document.getElementById(id).appendChild(element);
    }
    
}
async function addlikes(PostID){
        const response = await fetch("/requests/add", {
        method: "POST",
            headers: { "Accept": "application/json", "Content-Type": "application/json" },
            body: JSON.stringify({
                ID : PostID
            })
    });
    // если запрос прошел нормально
    if (response.ok) {
        var message = await response.json();
        var idlikecount = "likecount/" + PostID;
        var idlikebutton = "likebutton/" + PostID;
        var count = document.getElementById(idlikecount);
        var button = document.getElementById(idlikebutton);
        count.value = message.likes + " likes";
        button.innerHTML = message.btn;
    }
}