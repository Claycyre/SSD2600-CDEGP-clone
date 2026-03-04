function clearFilters() {
  // Uncheck all checkboxes
  document
    .querySelectorAll('input[name="Phases"]')
    //@ts-ignore
    .forEach((cb) => (cb.checked = false));
  document
    .querySelectorAll('input[name="types"]')
    //@ts-ignore
    .forEach((cb) => (cb.checked = false));

  // Reset sort to default (no value)
  //@ts-ignore
  document.querySelector('input[name="SortBy"][value=""]').checked = true;

  // Submit form
  //@ts-ignore
  document.getElementById("filterForm").submit();
}

function filtersOnSubmit(_: SubmitEvent) {
  window.location.hash = "#periodic-table";
}
