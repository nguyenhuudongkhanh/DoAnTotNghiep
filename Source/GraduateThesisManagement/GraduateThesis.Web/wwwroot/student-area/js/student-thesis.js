/// <reference path='../../lib/jquery/dist/jquery.min.js' />

function trim(str, ch) {
    var start = 0, end = str.length;

    while (start < end && str[start] === ch)
        ++start;

    while (end > start && str[end - 1] === ch)
        --end;

    return (start > 0 || end < str.length) ? str.substring(start, end) : str;
}


var thesisId = undefined;
var searchStudentUrl = undefined;
var getStudentByIdUrl = undefined;
var checkCanAddMemberUrl = undefined;

function clearSeachResult() {
    let searchResult = document.querySelector('#searchResult');
    searchResult.innerHTML = null;

    let defaultTrElement = document.createElement('tr');
    let tdElement = document.createElement('td');

    defaultTrElement.id = 'searchResult_defaultRow';
    tdElement.setAttribute('colspan', 4);
    tdElement.innerText = 'Chưa thực hiện tìm kiếm';

    defaultTrElement.appendChild(tdElement);
    searchResult.appendChild(defaultTrElement);
}

function loadToSearchResTable(items) {
    clearSeachResult();
    if (items.length == 0) {
        document.querySelector('#searchResult_defaultRow').style = null;   
    } else {
        document.querySelector('#searchResult_defaultRow').style = 'display: none';   
        let searchResult =document.querySelector("#searchResult");
        for (let item of items) {
            let idTdElement = document.createElement('td');
            let nameTdElement = document.createElement('td');
            let classNameTdElement = document.createElement('td');
            let selectTdElement = document.createElement('td');

            idTdElement.innerText = item['id'];
            nameTdElement.innerText = `${item['surname']} ${item['name']}`;
            classNameTdElement.innerText = item['studentClass']['name'];
            selectTdElement.innerHTML =  `<button type="button" class="btn btn-sm btn-success" onclick="selectStudent('${item['id']}')">Chọn</button>`;

            let newTrElement = document.createElement('tr');

            newTrElement.appendChild(idTdElement);
            newTrElement.appendChild(nameTdElement);
            newTrElement.appendChild(classNameTdElement);
            newTrElement.appendChild(selectTdElement);

            searchResult.appendChild(newTrElement);
        }
    }
}

function searchStudent(keyword) {
    $.ajax({
        url: `${searchStudentUrl}?keyword=${keyword}`,
        type: 'GET',
        success: function (data) {
            loadToSearchResTable(data);
        }
    });
}

function countSelectedStudent() {
    let trElements = document.getElementById('selectedStudent').getElementsByTagName('tr');
    let count = 0;

    for (let trElement of trElements)
        if (trElement.id !== 'selectedStudent_defaultRow')
            count++

    return count;
}

function removeFromSelectedInput(studentId) {
    let selectedInputElement = document.querySelector('#selectedStudentInput');

    let inputValue = selectedInputElement.value;
    inputValue = inputValue.replace(studentId, '').replace(';;', ';');
    inputValue = trim(inputValue, ";");

    selectedInputElement.value = inputValue;
}

function removeSelected(studentId) {
    document.querySelector(`#selected_${studentId}`).remove();
    removeFromSelectedInput(studentId);

    if (countSelectedStudent() == 0) {
        let selectedStudent = document.querySelector('#selectedStudent');
        let defaultTrElement = document.createElement("tr");
        let tdElement = document.createElement("td");

        defaultTrElement.id = 'selectedStudent_defaultRow';
        tdElement.setAttribute('colspan', 4);
        tdElement.innerText = 'Chưa chọn sinh viên';

        defaultTrElement.appendChild(tdElement);
        selectedStudent.appendChild(defaultTrElement);
    }
}

function addToSelectedTable(item) {
    let selectedStudent = document.querySelector('#selectedStudent');

    if (countSelectedStudent() == 0)
        selectedStudent.innerHTML = null;

    let idTdElement = document.createElement('td');
    let nameTdElement = document.createElement('td');
    let classNameTdElement = document.createElement('td');
    let selectTdElement = document.createElement('td');

    idTdElement.innerText = item['id'];
    nameTdElement.innerText = `${item['surname']} ${item['name']}`;
    classNameTdElement.innerText = item['studentClass']['name'];
    selectTdElement.innerHTML = `<button type="button" class="btn btn-sm btn-danger" onclick="removeSelected('${item['id']}')">Xóa</button>`;

    let newTrElement = document.createElement('tr');
    newTrElement.id = `selected_${item['id']}`;

    newTrElement.appendChild(idTdElement);
    newTrElement.appendChild(nameTdElement);
    newTrElement.appendChild(classNameTdElement);
    newTrElement.appendChild(selectTdElement);

    selectedStudent.appendChild(newTrElement);
}

function setToSelectedInput(studentId) {
    let selectedInputElement = document.querySelector('#selectedStudentInput');
    if (selectedInputElement.value == '')
        selectedInputElement.value = studentId;
    else
        selectedInputElement.value += `;${studentId}`;
}

function checkAndLoadSeletedTable(student) {
    $.ajax({
        url: checkCanAddMemberUrl,
        type: 'POST',
        data: { thesisId: thesisId, currentStudentNumber: countSelectedStudent() },
        success: function (data) {
            if (data['status'] == 'Failed') {
                alert('Bạn không thể thêm thành viên vì đã vượt quá số lượng quy định')
                return;
            }

            let check = document.querySelector(`#selected_${student['id']}`);
            if (check == null) {
                addToSelectedTable(student);
                setToSelectedInput(student['id']);
            } else {
                alert("Bạn đã thực hiện chọn thành viên này rồi!");
            }
            
        }
    })
}

function selectStudent(studentId) {
    $.ajax({
        url: `${getStudentByIdUrl}?studentId=${studentId}`,
        type: 'GET',
        success: function (data) {
            checkAndLoadSeletedTable(data)
        }
    })
}


$(function () {
    let timer;
    $('#studentKeyword').keyup(function () {
        clearTimeout(timer);
        var ms = 500; // milliseconds
        var val = this.value;
        timer = setTimeout(function () {
            var keyword = document.querySelector('#studentKeyword').value;
            searchStudent(keyword);
        }, ms);
    });
});