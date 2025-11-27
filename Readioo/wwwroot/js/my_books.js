$(document).ready(function () {
    $("#shelfList li").click(function () {
        $("#shelfList li").removeClass("active");
        $(this).addClass("active");

        let shelf = $(this).data("shelf");
        $("#sectionTitle").text($(this).text());

        if (shelf === "all") {
            $("#booksTableBody tr").show();
            return;
        }

        $("#booksTableBody tr").each(function () {
            let shelves = $(this).data("shelves").split(",");

            if (shelves.includes(shelf)) {
                $(this).show();
            } else {
                $(this).hide();
            }
        });
    });
});