<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.MultipleCategoriesFilterAction" CodeBehind="MultipleCategoriesFilterAction.ascx.cs" %>
<%@ Register TagPrefix="telerik" namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Button runat="server" ID="FilterButton" />
<div runat="server" class="events-categoryfilter-dialog">
    <telerik:RadTreeView runat="server" ID="CategoriesTreeView" CheckBoxes="True"
        TriStateCheckBoxes="False" CheckChildNodes="True"
        OnClientNodeChecked="OnClientNodeChecked" OnClientNodeClicking="OnClientNodeClicking" OnClientNodeClicked="OnClientNodeClicked"/>
    <asp:Button runat="server" ID="ApplyButton" OnClick="ApplyButton_Click"/>
</div>
<script type="text/javascript">
    jQuery(function ($) {
        var dlg = $('.events-categoryfilter-dialog').dialog({
            autoOpen: false,
            position: {
                my: 'right top',
                at: 'right bottom',
                of: $('#<%=this.FilterButton.ClientID %>')
            }
        });

        dlg.parent().appendTo($("form:first"));

        $('#<%=this.FilterButton.ClientID %>').click(function () {
            if ($('.events-categoryfilter-dialog').dialog("isOpen")) {
                $('.events-categoryfilter-dialog').dialog("close");
            }
            else {
                $('.events-categoryfilter-dialog').dialog("open");
            }
            return false;
        });
    });

    function DisableEnableAll(node) {
        var treeView = $find("<%=this.CategoriesTreeView.ClientID %>");
        var nodes = treeView.get_allNodes();

        if (node.get_level() == 0 && node.get_index() == 0) {
            // the root (all categories) node is checked
            var checked = node.get_checked();
            for (var i = 1; i < nodes.length; i++) {
                var attributes = nodes[i].get_attributes();
                if (nodes[i].get_nodes() != null) {
                    if (attributes.getAttribute("enabled") == "1") {
                        nodes[i].set_enabled(!checked);
                        nodes[i].set_checked(!checked);
                    }
                    else {
                        // node is not in the module setting filter.
                        nodes[i].set_enabled(false);
                        nodes[i].set_checked(false);
                    }
                }
            }
        }
        else {
            // check if all nodes are unchecked
            var allUnchecked = true;
            for (var i = 1; i < nodes.length; i++) {
                allUnchecked = allUnchecked && !nodes[i].get_checked();
            }

            if (allUnchecked) {
                nodes[0].set_checked(true);

                for (var i = 1; i < nodes.length; i++) {
                    if (nodes[i].get_nodes() != null) {
                        nodes[i].set_enabled(false);
                        nodes[i].set_checked(false);
                    }
                }
            }
        }
    }    
    
    function OnClientNodeClicked(sender, event) {
        var node = event.get_node();
        node.set_selected(false);
        return false;
    }

    function OnClientNodeClicking(sender, event) {
        var node = event.get_node();
        node.set_checked(!node.get_checked());
        node.set_selected(false);
        DisableEnableAll(node);
        return false;
    }

    function OnClientNodeChecked(sender, event) {
        var node = event.get_node();
        DisableEnableAll(node);
    }   
</script>