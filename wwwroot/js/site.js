document.addEventListener('DOMContentLoaded', function () {
    updateCartCount();
    setInterval(updateCartCount, 30000);
});

async function updateCartCount() {
    try {
        const response = await fetch('/Home/GetCartCount');
        const count = await response.json();
        const badge = document.getElementById('cartCount');
        if (badge) {
            badge.textContent = count;
            badge.style.display = count > 0 ? 'block' : 'none';
        }
    } catch (e) { }
}