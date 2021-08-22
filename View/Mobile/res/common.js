
window.MShop = window.MShop || {};


window.MShop.Helper = function() {


    this.expandSpec2 = function (owner) {
        var hasExp = $(owner).hasClass('spec-expand');

        if (hasExp) {
            $(owner).removeClass('spec-expand');

            $(owner).nextAll('.list-item').show();
        }
        else {
            $(owner).addClass('spec-expand');

            $(owner).nextAll('.list-item').hide();

        }
    }
}

window.MShopHelper = new window.MShop.Helper();