window.onload = function () {
    let addButtons = document.getElementsByClassName("add_btn");
    let removeButtons = document.getElementsByClassName("subtract_btn");
    let cart = document.getElementsByClassName("cartItem");

    // adding event listeners
    /*
    for (let i = 0; i < addButtons.length; i++) {
        addButtons[i].addEventListener('click', OnClickAdd);
    }
    
    for (let i = 0; i < removeButtons.length; i++) {
        removeButtons[i].addEventListener('click', OnClickSubtract);
    }
    */
    for (let i = 0; i < cart.length; i++) {
        cart[i].addEventListener('click', DumpIntoTheVoid);
    }
}

function OnClickAdd(event) {
    AddToCart(event.target.id);
}
function AddToCart(id) {
    let xhr = new XMLHttpRequest();
    let url = "/Cart/Add/" + id;
    xhr.open("POST", url);
    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");

    xhr.onreadystatechange = function () {
        if (this.readyState == XMLHttpRequest.DONE) {
            if (this.status == 200) {
                let data = JSON.parse(this.responseText);
                if (data.status == "success") {
                    window.location.href = "/Home/Index";
                }
            }
        }
    }
    xhr.send();
}

function OnClickSubtract(event) {
    RemoveFromCart(event.target.id);
}
function RemoveFromCart(id) {
    let xhr = new XMLHttpRequest();
    let url = "/Cart/Subtract/" + id;
    xhr.open("POST", url);
    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");

    xhr.onreadystatechange = function () {
        if (this.readyState == XMLHttpRequest.DONE) {
            if (this.status == 200) {
                let data = JSON.parse(this.responseText);
                if (data.status == "success") {
                    window.location.href = "/Home/Index";
                }
            }
        }
    }
    xhr.send();
}
/*
 * 
 * 
 * WIP
 * 
 * 
function UpdatePrice(event) {
    let target = event.currentTarget;
    let productId = target.id;

    let subtotalId = "subtotal" + productId;
    let unitpriceId = "unitprice" + productId;

    let value = document.getElementById(productId).value * 1;

    let unitprice = parseFloat(document.getElementById(unitpriceId).innerHTML.substring(1));

    if (value < 1 || !Number.isInteger(value)) {
        alert("Please input a correct quantity. You may remove the item by clicking on the delete icon on the right.");
        document.getElementById(productId).value = 1;
        document.getElementById(subtotalId).innerHTML = "$" + unitprice;

        subtotalgroup = document.getElementsByClassName("subtotals");

    }

    value = document.getElementById(productId).value * 1;

    AjaxUpdateCartDB(productId, value);

    let newsubtotal;

    if (isNaN(value)) {
        newsubtotal = 0 * unitprice;
    } else {
        newsubtotal = value * unitprice;
    }


    function formatNumber(num) {
        return num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,')
    }

    let finalsubtotal = formatNumber(newsubtotal.toFixed(2));

    document.getElementById(subtotalId).innerHTML = "$" + finalsubtotal;
}
function AjaxUpdateCartDB(productId, value) {
    let xhr = new XMLHttpRequest();

    xhr.open("POST", "/Cart/Update");
    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");

    xhr.onreadystatechange = function () {
        if (this.readyState === XMLHttpRequest.DONE) {
            if (this.status == 200) {
                let total = document.getElementById("totalPriceBottom");
                let data = JSON.parse(this.responseText);

                if (data.status == "success") {
                    total.innerHTML = "Total: $" + data.userCartAmt;
                }
            }
        }
    };


    let cartUpdate = {
        ProductId: productId,
        Quantity: value
    };
    xhr.send(JSON.stringify(cartUpdate));
}
*/

function DumpIntoTheVoid(event) {
    if (confirm("Confirm remove from cart?") == true) {
        DeleteFromCart(event.target.id);
    }
}
function DeleteFromCart(id)
{
    let xhr = new XMLHttpRequest();
    xhr.open("POST", "/Cart/Remove");
    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");
    xhr.onreadystatechange = function () {
        if (this.readyState === XMLHttpRequest.DONE) {
            let data = JSON.parse(this.responseText);
            if (this.status == 200) {
                if (data.status == "success") {
                    window.location.href = "/Cart";
                }
            }
        }
    }
    let itemToRemove = {
        ProductId: id
    }
    xhr.send(JSON.stringify(itemToRemove));
};
