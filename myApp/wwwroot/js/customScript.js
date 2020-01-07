function confirmDelete(uniqueId, isConfirmed) {
    if (!isConfirmed) {
        $('#' + 'deleteSpan_' + uniqueId).hide();
        $('#' + 'confirmDeleteSpan_' + uniqueId).show();
    } else {
        $('#' + 'deleteSpan_' + uniqueId).show();
        $('#' + 'confirmDeleteSpan_' + uniqueId).hide();
    }
}