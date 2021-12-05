import React from "react";

class ColumnNames extends React.Component {

    static columnNames = [];

    static getAll() {
        return this.columnNames;
    }

    static add(name) {
        let columnNames = this.columnNames.slice();
        this.columnNames = columnNames.concat(name);
    }

    static remove(name) {
        let columnNames = this.columnNames.slice();
        this.columnNames = columnNames.filter(c => c.id !== name.id)
    }

    static update(name) {
        let columnNames = this.columnNames.slice();
        columnNames.forEach((c, idx) => {
            if (c.id === name.id)
                columnNames.splice(idx, 1, name);
        })
        this.columnNames = columnNames;
    }
}

export default ColumnNames;
