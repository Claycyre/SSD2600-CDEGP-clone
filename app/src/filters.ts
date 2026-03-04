function clearFilters(e: Event) {
  const parentForm = (e.target as HTMLButtonElement).form;

  const phases = parentForm.elements.namedItem("Phases") as RadioNodeList;
  if (phases) {
    phases.forEach((cb) => (cb.checked = false));
  }

  const types = parentForm.elements.namedItem("types") as RadioNodeList;
  if (types) {
    types.forEach((cb) => (cb.checked = false));
  }

  const sortBy = parentForm.elements.namedItem("SortBy") as RadioNodeList;
  if (sortBy) {
    for (const [_, element] of sortBy.entries()) {
      if (element.value !== "") continue;

      element.checked = true;
      break;
    }
  }

  parentForm.requestSubmit();
}

function filtersOnSubmit(_: SubmitEvent) {
  window.location.hash = "#periodic-table";
}
