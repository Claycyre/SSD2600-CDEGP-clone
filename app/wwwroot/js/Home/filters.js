function clearFilters() {
  // Uncheck all checkboxes
  document
    .querySelectorAll('input[name="Phases"]')
    .forEach((cb) => (cb.checked = false));
  document
    .querySelectorAll('input[name="types"]')
    .forEach((cb) => (cb.checked = false));

  // Reset sort to default (no value)
  document.querySelector('input[name="SortBy"][value=""]').checked = true;

  // Submit form
  document.getElementById("filterForm").submit();
}
