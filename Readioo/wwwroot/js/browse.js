

document.addEventListener("DOMContentLoaded", () => {
    const books = document.querySelectorAll(".book-card");
    const genreItems = document.querySelectorAll("#genreFilter li");

    genreItems.forEach(li => {
        li.addEventListener("click", () => {
            genreItems.forEach(x => x.classList.remove("active"));
            li.classList.add("active");

            const genre = li.dataset.genre;

            books.forEach(b => {
                if (genre === "all" || b.dataset.genre.includes(genre)) {
                    b.style.display = "block";
                } else {
                    b.style.display = "none";
                }
            });
        });
    });
});
