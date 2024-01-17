window.getNavbarHeight = function () {
    const navbar = document.getElementById("navbar");
    if (navbar) {
        return navbar.clientHeight;
    }
    return 0; // Return a default value if the navbar is not found
}