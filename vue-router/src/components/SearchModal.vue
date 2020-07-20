<template>
<!-- Modal -->
<div class="modal fade" id="modalComponent" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
  <div class="modal-dialog" role="document">
    <div v-bind:style="{'background-color':primaryColor}"  class="modal-content">
      <div class="modal-header" v-bind:style="{'background-color':primaryColor}">
        <h5 class="modal-title" id="exampleModalLabel">Search username</h5>
          <button type="button" class="close" data-dismiss="modal" aria-label="Close">
            <span aria-hidden="true">&times;</span>
          </button>
      </div>
      <div class="modal-body">
        <form>
          <div class="form-group">
            <label for="searchUsername">Username</label>
            <input type="text" class="form-control" id="searchUsername" placeholder="Search contact by username" v-model="searchTerm">
          </div>
        </form>
      </div>
      <div class="modal-footer" v-bind:style="{'background-color':primaryColor}">
        <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
        <button type="button" class="btn btn-primary" v-on:click=AddContact>Add Contact</button>
      </div>
    </div>
  </div>
 </div>
</template>

<script>
import mixins from '../mixins.js'
import axios from 'axios'

export default {
  name: 'searchmodal',
  props: ['primaryColor'],
  mixins: [mixins],
  components: {
  },
  data () {
    return {
      color: this.primaryColor,
      searchTerm: null,
      userListBySearchTerm: [],
      selectedUserToAdd: null
    }
  },
  mounted () {
  },
  methods: {
    searchContactByName: function () {
      let self = this
      axios.get(`${this.apiUrl}api/user/searchContactByName?user_id=${localStorage.userId}&contact_name=${this.searchTerm}`)
        .then(function (response) {
          console.log(response.data.Values)
          self.userListBySearchTerm = response.data.Values
        }).catch(error => {
          console.log(error.response)
        })
    },
    AddContact: function () {
      console.log('test')
    }
  },
  watch: {
    searchTerm: function () {
      if (this.searchTerm && this.searchTerm.length > 3) {
        this.searchContactByName()
      }
    }
  }
}
</script>

<style>
select {
  margin: 50px;
  width: 150px;
  padding: 5px 35px 5px 5px;
  font-size: 16px;
  border: 1px solid #CCC;
  height: 34px;
  -webkit-appearance: none;
  -moz-appearance: none;
  appearance: none;
  background: url(http://www.stackoverflow.com/favicon.ico) 96% / 15% no-repeat #EEE;
}

/* CAUTION: Internet Explorer hackery ahead */

select::-ms-expand {
    display: none; /* Remove default arrow in Internet Explorer 10 and 11 */
}

/* Target Internet Explorer 9 to undo the custom arrow */
@media screen and (min-width:0\0) {
    select {
        background: none\9;
        padding: 5px\9;
    }
}
</style>
