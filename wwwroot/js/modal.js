function showModal(header, message, bootstrapClass, proceedFunction) {

    if (!message)
        throw 'Message must not be empty';

    if (!header)
        throw 'Header must not be empty';

    if (bootstrapClass != 'primary' &&
        bootstrapClass != 'warning' &&
        bootstrapClass != 'danger' &&
        bootstrapClass != 'success')
        throw 'Provided bootstrap class not supported';

    let modalContent = `

<button hidden id="launchModalBtn" type="button" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#staticBackdrop">
</button>

    <div class="modal fade" id="staticBackdrop" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title fw-bold" id="staticBackdropLabel">${header}</h5>
            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
          </div>
          <div class="modal-body text-center">
            ${message}
          </div>
          <div class="modal-footer">
            <div class="d-flex w-100">
                <button type="button" class="w-25 btn btn-secondary" data-bs-dismiss="modal">Close</button>
                <div class="w-50"></div>
                <button type="button" onclick="proceedFunction" class="w-25 btn btn-${bootstrapClass}">Proceed</button>
            </div>
          </div>
        </div>
      </div>
    </div>
`;

    let modal = $('#modal');
    modal.html(modalContent);

    $('#launchModalBtn').click();

}