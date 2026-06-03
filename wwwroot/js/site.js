(function () {
    function attachTableSearch(inputId, tableId) {
        var input = document.getElementById(inputId);
        var table = document.getElementById(tableId);

        if (!input || !table) {
            return;
        }

        input.addEventListener("input", function () {
            var query = input.value.trim().toLocaleLowerCase("tr-TR");
            var rows = table.querySelectorAll("tbody tr");

            rows.forEach(function (row) {
                var text = row.textContent.toLocaleLowerCase("tr-TR");
                row.hidden = query.length > 0 && !text.includes(query);
            });
        });
    }

    document.addEventListener("DOMContentLoaded", function () {
        attachTableSearch("courseSearch", "coursesTable");
        attachTableSearch("taskSearch", "tasksTable");
        attachTableSearch("examSearch", "examsTable");
    });
})();
