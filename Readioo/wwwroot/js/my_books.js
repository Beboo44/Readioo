

document.addEventListener("DOMContentLoaded", () => {
    const shelfItems = document.querySelectorAll(".shelf-list li");
    const rows = document.querySelectorAll("#booksTableBody tr");
    const sectionTitle = document.getElementById("sectionTitle");

    shelfItems.forEach(li => {
        li.addEventListener("click", () => {
            const shelf = li.dataset.shelf;

            shelfItems.forEach(x => x.classList.remove("active"));
            li.classList.add("active");

            rows.forEach(row => {
                const shelves = row.dataset.shelves.split(",").map(s => s.trim());

                if (shelf === "all" || shelves.includes(shelf)) {
                    row.style.display = "";
                } else {
                    row.style.display = "none";
                }
            });

            sectionTitle.textContent =
             shelf === "all" ? "All Books" :
             shelf === "read" ? "Read Books" :
             shelf === "currently-reading" ? "Currently Reading" :
             shelf === "to-read" ? "Want to Read" :
             "Favorite Books";
        });
    });
});
